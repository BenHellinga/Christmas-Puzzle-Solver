namespace PuzzleSolver;



internal class Solver
{
    private List<Piece> pieces;
    private Board board;
    private char[] progress; // scratch buffer used to print search progress to the console



    static void Main(string[] args)
    {
        Solver solver = new();

        DateTime start = DateTime.Now;

        if (solver.solve(0, makeTypes()))
            solver.board.print();
        else
            Console.WriteLine("No solution found");

        DateTime end = DateTime.Now;

        Console.WriteLine("Finished in " + (end - start));
    }



    public Solver()
    {
        board = new(9, 6);
        pieces = [];
        progress = "0 0/0 0/0, 0 0/0 0/0".ToCharArray();

        Tile.generateTiles();
        generatePieces();
    }



    private void generatePieces()
    {
        // each piece is width, height, a tile id per cell, a rotation per cell, then the piece's id
        pieces.Add(new(3, 2, [3, 2, 3, 3, 2, 6],
                             [3, 0, 0, 2, 2, 1], 0));

        pieces.Add(new(4, 2, [3, 2, 4, 5, 3, 6, 0, 0],
                             [3, 0, 0, 0, 2, 1, 0, 0], 1));

        pieces.Add(new(4, 2, [5, 2, 3, 7, 0, 6, 2, 5],
                             [2, 0, 0, 2, 0, 2, 2, 0], 2));

        pieces.Add(new(4, 2, [5, 2, 2, 8, 7, 3, 6, 0],
                             [2, 0, 0, 1, 0, 2, 1, 0], 3));

        pieces.Add(new(5, 2, [5, 4, 2, 4, 8, 0, 7, 5, 0, 0],
                             [2, 0, 0, 0, 1, 0, 0, 1, 0, 0], 4));

        pieces.Add(new(3, 3, [3, 2, 8, 3, 2, 0, 7, 5, 0],
                             [3, 0, 1, 2, 1, 0, 0, 1, 0], 5));

        pieces.Add(new(3, 3, [3, 3, 7, 6, 2, 3, 0, 0, 9],
                             [3, 0, 2, 2, 2, 0, 0, 0, 1], 6));

        pieces.Add(new(3, 3, [3, 3, 0, 6, 1, 5, 0, 8, 0],
                             [3, 0, 0, 2, 0, 0, 0, 2, 0], 7));

        pieces.Add(new(3, 3, [3, 2, 8, 2, 3, 0, 9, 0, 0],
                             [3, 0, 1, 3, 1, 0, 1, 0, 0], 8));
    }



    // private methods



    // 9x6 empty board, all cells start unoccupied
    private static List<List<int>> makeTypes()
    {
        return [[0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0]];
    }



    // returns a copy of types with this piece stamped onto it, marking overlaps as filled
    private List<List<int>> placePiece(List<List<int>> types, Piece piece)
    {
        List<List<int>> newTypes = [];

        for (int y = 0; y < board.height; y++)
        {
            newTypes.Add([]);
            for (int x = 0; x < board.width; x++)
                newTypes[y].Add(types[y][x]);
        }

        for (int y = 0; y < piece.height; y++)
        for (int x = 0; x < piece.width; x++)
        {
            if (piece.tiles[y * piece.width + x].type == 0) continue;

            if (newTypes[y + piece.y][x + piece.x] == 0)
                newTypes[y + piece.y][x + piece.x] = piece.tiles[y * piece.width + x].type;
            else
                newTypes[y + piece.y][x + piece.x] = 1;
        }

        return newTypes;
    }



    // public methods



    // recursive backtracking search, tries every position for the piece in each of its
    // 8 orientations (4 rotations x mirrored/not) at every board position, and recurses on success
    public bool solve(int piece, List<List<int>> types)
    {
        if (piece == 9) return true;


        Console.WriteLine();
        board.print();
        Console.WriteLine(piece);


        if (piece < 3) Console.WriteLine(progress);

        if (piece < 2)
        {
            progress[piece * 11] = '0';
            progress[piece * 11 + 4] = (char)(board.height - pieces[piece].height + 48);
            progress[piece * 11 + 8] = (char)(board.width - pieces[piece].width + 48);
        }

        for (int y = 0; y < board.height - pieces[piece].height + 1; y++)
        for (int x = 0; x < board.width - pieces[piece].width + 1; x++)
        {
            if (piece < 2)
            {
                progress[piece * 11 + 2] = (char)(y + 48);
                progress[piece * 11 + 6] = (char)(x + 48);
            }

            if (board.place(pieces[piece], x, y, types))
            {
                if (solve(piece + 1, placePiece(types, pieces[piece])))
                    return true;
                else
                    board.pieces.RemoveAt(piece);
            }
        }

        pieces[piece].rotate(1);

        if (piece < 2)
        {
            progress[piece * 11] = '1';
            progress[piece * 11 + 4] = (char)(board.height - pieces[piece].height + 48);
            progress[piece * 11 + 8] = (char)(board.width - pieces[piece].width + 48);
        }

        for (int y = 0; y < board.height - pieces[piece].height + 1; y++)
        for (int x = 0; x < board.width - pieces[piece].width + 1; x++)
        {
            if (piece < 2)
            {
                progress[piece * 11 + 2] = (char)(y + 48);
                progress[piece * 11 + 6] = (char)(x + 48);
            }

            if (board.place(pieces[piece], x, y, types))
            {
                if (solve(piece + 1, placePiece(types, pieces[piece])))
                    return true;
                else
                    board.pieces.RemoveAt(piece);
            }
        }

        pieces[piece].rotate(1);

        if (piece < 2)
        {
            progress[piece * 11] = '2';
            progress[piece * 11 + 4] = (char)(board.height - pieces[piece].height + 48);
            progress[piece * 11 + 8] = (char)(board.width - pieces[piece].width + 48);
        }

        for (int y = 0; y < board.height - pieces[piece].height + 1; y++)
        for (int x = 0; x < board.width - pieces[piece].width + 1; x++)
        {
            if (piece < 2)
            {
                progress[piece * 11 + 2] = (char)(y + 48);
                progress[piece * 11 + 6] = (char)(x + 48);
            }

            if (board.place(pieces[piece], x, y, types))
            {
                if (solve(piece + 1, placePiece(types, pieces[piece])))
                    return true;
                else
                    board.pieces.RemoveAt(piece);
            }
        }

        pieces[piece].rotate(1);

        if (piece < 2)
        {
            progress[piece * 11] = '3';
            progress[piece * 11 + 4] = (char)(board.height - pieces[piece].height + 48);
            progress[piece * 11 + 8] = (char)(board.width - pieces[piece].width + 48);
        }

        for (int y = 0; y < board.height - pieces[piece].height + 1; y++)
        for (int x = 0; x < board.width - pieces[piece].width + 1; x++)
        {
            if (piece < 2)
            {
                progress[piece * 11 + 2] = (char)(y + 48);
                progress[piece * 11 + 6] = (char)(x + 48);
            }

            if (board.place(pieces[piece], x, y, types))
            {
                if (solve(piece + 1, placePiece(types, pieces[piece])))
                    return true;
                else
                    board.pieces.RemoveAt(piece);
            }
        }

        // back to rotation 0, now try the mirrored side through all 4 rotations again
        pieces[piece].rotate(1);
        pieces[piece].mirror();

        if (piece < 2)
        {
            progress[piece * 11] = '4';
            progress[piece * 11 + 4] = (char)(board.height - pieces[piece].height + 48);
            progress[piece * 11 + 8] = (char)(board.width - pieces[piece].width + 48);
        }

        for (int y = 0; y < board.height - pieces[piece].height + 1; y++)
        for (int x = 0; x < board.width - pieces[piece].width + 1; x++)
        {
            if (piece < 2)
            {
                progress[piece * 11 + 2] = (char)(y + 48);
                progress[piece * 11 + 6] = (char)(x + 48);
            }

            if (board.place(pieces[piece], x, y, types))
            {
                if (solve(piece + 1, placePiece(types, pieces[piece])))
                    return true;
                else
                    board.pieces.RemoveAt(piece);
            }
        }

        pieces[piece].rotate(1);

        if (piece < 2)
        {
            progress[piece * 11] = '5';
            progress[piece * 11 + 4] = (char)(board.height - pieces[piece].height + 48);
            progress[piece * 11 + 8] = (char)(board.width - pieces[piece].width + 48);
        }

        for (int y = 0; y < board.height - pieces[piece].height + 1; y++)
        for (int x = 0; x < board.width - pieces[piece].width + 1; x++)
        {
            if (piece < 2)
            {
                progress[piece * 11 + 2] = (char)(y + 48);
                progress[piece * 11 + 6] = (char)(x + 48);
            }

            if (board.place(pieces[piece], x, y, types))
            {
                if (solve(piece + 1, placePiece(types, pieces[piece])))
                    return true;
                else
                    board.pieces.RemoveAt(piece);
            }
        }

        pieces[piece].rotate(1);

        if (piece < 2)
        {
            progress[piece * 11] = '6';
            progress[piece * 11 + 4] = (char)(board.height - pieces[piece].height + 48);
            progress[piece * 11 + 8] = (char)(board.width - pieces[piece].width + 48);
        }

        for (int y = 0; y < board.height - pieces[piece].height + 1; y++)
        for (int x = 0; x < board.width - pieces[piece].width + 1; x++)
        {
            if (piece < 2)
            {
                progress[piece * 11 + 2] = (char)(y + 48);
                progress[piece * 11 + 6] = (char)(x + 48);
            }

            if (board.place(pieces[piece], x, y, types))
            {
                if (solve(piece + 1, placePiece(types, pieces[piece])))
                    return true;
                else
                    board.pieces.RemoveAt(piece);
            }
        }

        pieces[piece].rotate(1);

        if (piece < 2)
        {
            progress[piece * 11] = '7';
            progress[piece * 11 + 4] = (char)(board.height - pieces[piece].height + 48);
            progress[piece * 11 + 8] = (char)(board.width - pieces[piece].width + 48);
        }

        for (int y = 0; y < board.height - pieces[piece].height + 1; y++)
        for (int x = 0; x < board.width - pieces[piece].width + 1; x++)
        {
            if (piece < 2)
            {
                progress[piece * 11 + 2] = (char)(y + 48);
                progress[piece * 11 + 6] = (char)(x + 48);
            }

            if (board.place(pieces[piece], x, y, types))
            {
                if (solve(piece + 1, placePiece(types, pieces[piece])))
                    return true;
                else
                    board.pieces.RemoveAt(piece);
            }
        }

        pieces[piece].rotate(1);
        return false;
    }
}
