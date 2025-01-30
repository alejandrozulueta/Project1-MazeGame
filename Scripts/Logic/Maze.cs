public class Maze : IMaze
{
    public int Length { get; private set; }
    public int Width { get; private set; }
    public Cell[,] Cells { get; set; }
    public (int, int) Scape { get; private set; }

    public Maze(int lenght, int width, Cell[,] cells, (int, int) scape)
    {
        Length = lenght;
        Width = width;
        Cells = cells;
        Scape = scape;
    }

    public Maze((int length, int width) dimensions)
    {
        Length = dimensions.length;
        Width = dimensions.width;
        Cells = new Cell[Length, Width];

        GenerateMaze();
    }

    private void GenerateMaze()
    {
        InitializeMaze();
        List<(int x, int y)> walls = new List<(int x, int y)>();
        int startX = Randoms.random.Next(Width);
        int startY = Randoms.random.Next(Length);
        Cells[startY, startX].IsWall = false;
        AddAdjacentWalls(startX, startY, walls);

        while (walls.Count > 0)
        {
            var (x, y) = walls[Randoms.random.Next(walls.Count)];
            walls.Remove((x, y));

            if (CanRemoveWall(x, y))
            {
                Cells[y, x].IsWall = false;
                AddAdjacentWalls(x, y, walls);
            }
        }

        SetGoal();
    }

    private void InitializeMaze()
    {
        for (int y = 0; y < Length; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Cells[y, x] = new Cell { IsWall = true };
            }
        }
    }

    private void AddAdjacentWalls(int x, int y, List<(int x, int y)> walls)
    {
        if (x > 1)
            walls.Add((x - 1, y));
        if (x < Width - 2)
            walls.Add((x + 1, y));
        if (y > 1)
            walls.Add((x, y - 1));
        if (y < Length - 2)
            walls.Add((x, y + 1));
    }

    private bool CanRemoveWall(int x, int y)
    {
        int adjacentPaths = 0;

        if (x > 0 && !Cells[y, x - 1].IsWall)
            adjacentPaths++;
        if (x < Width - 1 && !Cells[y, x + 1].IsWall)
            adjacentPaths++;
        if (y > 0 && !Cells[y - 1, x].IsWall)
            adjacentPaths++;
        if (y < Length - 1 && !Cells[y + 1, x].IsWall)
            adjacentPaths++;

        return adjacentPaths == 1;
    }

    private void SetGoal()
    {
        int scapeY = Randoms.random.Next(Length / 3, Length - 2);
        int scapeX = Randoms.random.Next(1, Width - 2);

        if (Cells[scapeY, scapeX].IsWall)
        {
            SetGoal();
        }
        else
        {
            Cells[scapeY, scapeX].IsGoal = true;
            Scape = (scapeX, scapeY);
        }
    }
}
