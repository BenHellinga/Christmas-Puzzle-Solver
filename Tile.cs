namespace PuzzleSolver;



internal class Tile
{
    public int id;
    public int rotation;
    public int type;

    private static List<List<string>> tiles = null!; // set in generateTiles(), before any print() call



    // constructor



    public Tile(int Id, int Rotation)
    {
        id = Id;
        rotation = Rotation;

        if (id == 0) type = 0;
        else if (id <= 5) type = 1;
        else if (id == 7) type = 6 + rotation;
        else type = 2 + rotation;
    }



    // public methods



    public static void generateTiles()
    {
        tiles = [];

        // tiles.txt is copied next to the built exe, so this works regardless of where the project lives
        string[] lines = File.ReadAllLines("Tiles.txt");

        for (int i = 0; i < 10 * 6; i += 6)
            tiles.Add([lines[i], lines[i + 1], lines[i + 2], lines[i + 3], lines[i + 4]]);
    }



    public void print()
    {
        for (int i = 0; i < 5; i++)
        {
            print(i);
            Console.WriteLine();
        }
    }



    public void print(int line)
    {
        string s = "";

        // rotation picks which edge of the 5x5 ascii tile to read as the top row
        switch (rotation)
        {
            case 0:
                for (int i = 0; i < 5; i++)
                    s += tiles[id][line][i] + " ";
                break;

            case 1:
                for (int i = 0; i < 5; i++)
                    s += tiles[id][4 - i][line] + " ";
                break;

            case 2:
                for (int i = 0; i < 5; i++)
                    s += tiles[id][4 - line][4 - i] + " ";
                break;

            case 3:
                for (int i = 0; i < 5; i++)
                    s += tiles[id][i][4 - line] + " ";
                break;
        }

        Console.Write(s);
    }



    public void rotate(int r)
    {
        rotation += r;

        // each tile id has its own symmetry, so rotation wraps at a different point per id
        switch (id)
        {
            case 0: rotation = 0; break;
            case 1: rotation = 0; break;
            case 2: rotation %= 4; break;
            case 3: rotation %= 4; break;
            case 4: rotation %= 2; break;
            case 5: rotation %= 4; break;
            case 6: rotation %= 4; break;
            case 7: rotation %= 4; break;
            case 8: rotation %= 4; break;
            case 9: rotation %= 4; break;
        }
    }



    public void mirror()
    {
        // ids 0, 1, 4 are symmetric, mirroring does nothing
        if (id == 0 || id == 1 || id == 4) return;

        if (id == 2)
        {
            if (rotation == 0) { rotation = 2; return; }
            if (rotation == 2) { rotation = 0; return; }
            return;
        }

        if (id == 5)
        {
            if (rotation == 1) { rotation = 3; return; }
            if (rotation == 3) { rotation = 1; return; }
            return;
        }

        else if (rotation == 0) rotation = 1;
        else if (rotation == 1) rotation = 0;
        else if (rotation == 2) rotation = 3;
        else if (rotation == 3) rotation = 2;

        // ids 8 and 9 are mirror images of each other, so mirroring swaps which one this tile is
        if (id == 8) { id = 9; return; }
        if (id == 9) { id = 8; return; }
    }
}



/*  Piece tiles
 * 
 *  0:0
 *  _ _ _ _ _
 *  _ _ _ _ _
 *  _ _ _ _ _
 *  _ _ _ _ _
 *  _ _ _ _ _
 *
 *  1:0
 *  _ _ _ _ _
 *  _ _ _ _ _
 *  _ _ _ _ _
 *  _ _ _ _ _
 *  _ _ _ _ _
 *
 *  2:0        2:1          2:2         2:3
 *  x x x x x   _ _ _ _ x   _ _ _ _ _   x _ _ _ _
 *  _ _ _ _ _   _ _ _ _ x   _ _ _ _ _   x _ _ _ _
 *  _ _ _ _ _   _ _ _ _ x   _ _ _ _ _   x _ _ _ _
 *  _ _ _ _ _   _ _ _ _ x   _ _ _ _ _   x _ _ _ _
 *  _ _ _ _ _   _ _ _ _ x   x x x x x   x _ _ _ _
 *
 *  3:0         3:1         3:2         3:3
 *  x x x x x   _ _ _ _ x   x _ _ _ _   x x x x x
 *  _ _ _ _ x   _ _ _ _ x   x _ _ _ _   x _ _ _ _
 *  _ _ _ _ x   _ _ _ _ x   x _ _ _ _   x _ _ _ _
 *  _ _ _ _ x   _ _ _ _ x   x _ _ _ _   x _ _ _ _
 *  _ _ _ _ x   x x x x x   x x x x x   x _ _ _ _
 * 
 *  4:0         4:1
 *  x x x x x   x _ _ _ x
 *  _ _ _ _ _   x _ _ _ x
 *  _ _ _ _ _   x _ _ _ x
 *  _ _ _ _ _   x _ _ _ x
 *  x x x x x   x _ _ _ x
 * 
 *  5:0         5:1         5:2         5:3
 *  x x x x x   x _ _ _ x   x x x x x   x x x x x
 *  _ _ _ _ x   x _ _ _ x   x _ _ _ _   x _ _ _ x
 *  _ _ _ _ x   x _ _ _ x   x _ _ _ _   x _ _ _ x
 *  _ _ _ _ x   x _ _ _ x   x _ _ _ _   x _ _ _ x
 *  x x x x x   x x x x x   x x x x x   x _ _ _ x
 * 
 *  6:0         6:1         6:2         6:3
 *  x x x _ _   _ _ _ _ x   x _ _ _ _   _ _ x x x
 *  _ _ _ x _   _ _ _ _ x   x _ _ _ _   _ x _ _ _
 *  _ _ _ _ x   _ _ _ _ x   x _ _ _ _   x _ _ _ _
 *  _ _ _ _ x   _ _ _ x _   _ x _ _ _   x _ _ _ _
 *  _ _ _ _ x   x x x _ _   _ _ x x x   x _ _ _ _
 * 
 *  7:0         7:1         7:2         7:3
 *  _ _ _ x x   _ _ _ _ _   _ _ _ _ _   x x _ _ _
 *  _ _ _ _ x   _ _ _ _ _   _ _ _ _ _   x _ _ _ _
 *  _ _ _ _ _   _ _ _ _ _   _ _ _ _ _   _ _ _ _ _
 *  _ _ _ _ _   _ _ _ _ x   x _ _ _ _   _ _ _ _ _
 *  _ _ _ _ _   _ _ _ x x   x x _ _ _   _ _ _ _ _
 *  
 *  8:0         8:1         8:2         8:3
 *  x x x _ _   x x x x x   x _ _ _ x   _ _ x x x
 *  x _ _ x _   _ _ _ _ x   x _ _ _ x   _ x _ _ _
 *  x _ _ _ x   _ _ _ _ x   x _ _ _ x   x _ _ _ _
 *  x _ _ _ x   _ _ _ x _   _ x _ _ x   x _ _ _ _
 *  x _ _ _ x   x x x _ _   _ _ x x x   x x x x x
 * 
 *  9:0         9:1         9:2         9:3
 *  x x x _ _   x _ _ _ x   x x x x x   _ _ x x x
 *  _ _ _ x _   x _ _ _ x   x _ _ _ _   _ x _ _ x
 *  _ _ _ _ x   x _ _ _ x   x _ _ _ _   x _ _ _ x
 *  _ _ _ _ x   x _ _ x _   _ x _ _ _   x _ _ _ x
 *  x x x x x   x x x _ _   _ _ x x x   x _ _ _ x
 * 
 */
