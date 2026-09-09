namespace PuzzleSolver;



internal class Board
{
    public int width;
    public int height;
    public List<Piece> pieces;



    // constructor



    public Board(int Width, int Height)
    {
        width = Width;
        height = Height;

        pieces = [];
    }



    // public methods



    public bool place(Piece piece, int x, int y, List<List<int>> types)
    {
        if (!checkCollision(piece, x, y, types)) return false;

        pieces.Add(piece);

        piece.x = x;
        piece.y = y;

        return true;
    }



    public void print()
    {
        // border, top row of x's
        Console.Write("x");
        for (int i = 0; i < width * 5 + 3; i++)
            Console.Write(" x");
        Console.WriteLine();

        Console.Write("x ");
        for (int i = 0; i < width * 5 + 1; i++)
            Console.Write("  ");
        Console.WriteLine("  x");

        // 5 console lines per tile row, ask each piece to print itself if it covers this cell
        for (int i = 0; i < height * 5; i++)
        {
            Console.Write("x   ");

            int x = 0;
            bool piecePrinted;

            while (x < width)
            {
                piecePrinted = false;
                foreach (Piece piece in pieces)
                {
                    if (piece.x <= x && piece.x + piece.width > x && piece.y <= i / 5 && piece.y + piece.height - 1 >= i / 5)
                    {
                        piecePrinted = piece.print(i - piece.y * 5, x - piece.x);
                        if (!piecePrinted) continue;
                        break;
                    }
                }

                if (!piecePrinted)
                    Console.Write("          ");

                piecePrinted = false;
                x++;
            }

            Console.WriteLine("  x");
        }

        Console.Write("x ");
        for (int i = 0; i < width * 5 + 1; i++)
            Console.Write("  ");
        Console.WriteLine("  x");

        Console.Write("x");
        for (int i = 0; i < width * 5 + 3; i++)
            Console.Write(" x");
        Console.WriteLine();
    }



    // private methods



    // types grid tracks what's already occupying each board cell,
    // 0 empty, 1 solid body, 6+ a locked interlocking edge, difference of 4 means a tab matches a slot
    private static bool checkCollision(Piece piece, int px, int py, List<List<int>> types)
    {
        for (int y = 0; y < piece.height; y++)
        for (int x = 0; x < piece.width; x++)
        {
            if (types[py + y][px + x] == 0 || piece.tiles[y * piece.width + x].type == 0) continue;
            if (types[py + y][px + x] == 1 || piece.tiles[y * piece.width + x].type == 1) return false;
            if (types[py + y][px + x] >= 6 && piece.tiles[y * piece.width + x].type >= 6) return false;

            if (Math.Abs(types[py + y][px + x] - piece.tiles[y * piece.width + x].type) == 4) continue;
            return false;
        }
        
        return true;
    }
}
