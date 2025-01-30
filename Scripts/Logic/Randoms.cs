public static class Randoms
{
    public static Random random = new Random();

    public static (int x, int y) RandomPosition(IMaze maze)
    {
        int x = random.Next(0, maze.Width);
        int y = random.Next(0, maze.Length);

        if (
            maze.Cells[y, x].IsWall
            || maze.Cells[y, x].IsTramp
            || maze.Cells[y, x].IsCharacter
            || maze.Cells[y, x].IsEnemy
            || maze.Cells[y, x].IsGoal
        )
        {
            return RandomPosition(maze);
        }

        return (x, y);
    }
}
