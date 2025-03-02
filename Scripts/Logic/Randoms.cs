public static class Randoms
{
    public static Random random = new Random();

    public static (int x, int y) RandomPosition(IMaze maze)
    {
        int x = random.Next(0, maze.Width);
        int y = random.Next(0, maze.Length);

        if (!maze[y, x].IsEmpty || maze[y, x].IsTramp || maze[y, x].IsGoal)
        {
            return RandomPosition(maze);
        }

        return (x, y);
    }
}
