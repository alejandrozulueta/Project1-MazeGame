public class Vision
{
    public int rangeOfVision;
    public List<(int x, int y)> ActualVision;
    private (int x, int y) lastPosition;

    public Vision(int range, (int x, int y) position, IMaze maze)
    {
        rangeOfVision = range;
        lastPosition = position;
        ActualVision = [];

        UpdateVision(position, maze);
    }

    public void UpdateVision((int x, int y) position, IMaze maze)
    {
        if (lastPosition == position && ActualVision.Count > 0)
            return;

        int startX = position.x;
        int startY = position.y;
        lastPosition = position;

        for (int dx = -rangeOfVision; dx <= rangeOfVision; dx++)
        {
            for (int dy = -rangeOfVision; dy <= rangeOfVision; dy++)
            {
                int x = startX + dx;
                int y = startY + dy;

                if (x >= 0 && x < maze.Width && y >= 0 && y < maze.Length)
                {
                    if (ActualVision.Contains((x, y)))
                        continue;

                    ActualVision.Add((x, y));
                }
            }
        }
    }
}
