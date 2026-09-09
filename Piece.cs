namespace PuzzleSolver;



internal class Piece
{
    public int id;
    public int x;
    public int y;
    public int width;
    public int height;
    public int rotation;
    public int mirrored;
    public List<Tile> tiles = null!; // set in populateShape(), called from the constructor



    // constructor



    public Piece(int Width, int Height, List<int> Tiles, List<int> Rotations, int Id)
    {
        width = Width;
        height = Height;
        id = Id;
        rotation = 0;

        populateShape(Tiles, Rotations);
    }



    // public methods



    public void print()
    {
        for (int i = 0; i < height * 5; i++)
        {
            print(i);
            Console.WriteLine();
        }
    }



    public void print(int line)
    {
        for (int x = 0; x < width; x++)
            tiles[line / 5 * width + x].print(line % 5);
    }



    public bool print(int line, int tile)
    {
        if (tiles[line / 5 * width + tile].id == 0) return false;

        tiles[line / 5 * width + tile].print(line % 5);
        return true;
    }



    public void rotate(int r)
    {
        rotation += r;

        // rebuild the tile list rotated 90 degrees, r times, swapping width/height each time
        for (int i = 0; i < r; i++)
        {
            List<Tile> newTiles = [];

            for (int x = 0; x < width; x++)
                for (int y = height - 1; y >= 0; y--)
                    newTiles.Add(tiles[y * width + x]);

            tiles = newTiles;

            int temp = height;
            height = width;
            width = temp;
        }

        for (int i = 0; i < width * height; i++)
        {
            tiles[i].rotate(r);
            if (tiles[i].type >= 2)
                tiles[i].type = (tiles[i].type - 2) / 4 * 4 + 2 + tiles[i].rotation;
        }
    }



    public void mirror()
    {
        mirrored = mirrored == 0 ? 1 : 0;

        // flip the tile list vertically (top row becomes bottom row, etc)
        List<Tile> newTiles = [];

        for (int y = height - 1; y >= 0; y--)
            for (int x = 0; x < width; x++)
                newTiles.Add(tiles[y * width + x]);

        tiles = newTiles;

        for (int i = 0; i < width * height; i++)
        {
            tiles[i].mirror();
            if (tiles[i].type >= 2)
                tiles[i].type = (tiles[i].type - 2) / 4 * 4 + 2 + tiles[i].rotation;
        }
    }



    // private methods



    private void populateShape(List<int> Tiles, List<int> Rotations)
    {
        tiles = [];

        for (int i = 0; i < width * height; i++)
            tiles.Add(new(Tiles[i], Rotations[i]));
    }
}
