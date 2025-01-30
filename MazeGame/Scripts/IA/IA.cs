public class PlayerIA : DataPlayer
{
    IAStrategies strategie;
    List<(int x, int y)> ExploredPos;
    LinkedList<(GameKey, (int x, int y))> NextMovs;
    int PathCount = 0;

    public PlayerIA()
    {
        strategie = new IAStrategies();

        ExploredPos = new List<(int x, int y)>();
        NextMovs = new LinkedList<(GameKey, (int x, int y))>();
    }

    public GameKey Movement(IMaze maze)
    {
        (int scapeX, int scapeY) Goal;

        if (NextMovs.Count == 0)
        {
            PathCount++;

            if (PathCount > 3)
            {
                Goal = maze.Scape;
            }
            else
            {
                Goal = SelectEnd(maze);
            }

            int[,] num = strategie.Lee(maze, CurrentPosition, Goal);
            NextMovs = strategie.FindPath(num, Goal);
        }

        var distance = GeneralMethods.Distance(CurrentPosition, NextMovs.Last!.Value.Item2);

        if (distance != 1)
        {
            PathCount++;
            NextMovs.Clear();

            if (PathCount > 3)
            {
                Goal = maze.Scape;
            }
            else
            {
                Goal = SelectEnd(maze);
            }

            int[,] num = strategie.Lee(maze, CurrentPosition, Goal);
            NextMovs = strategie.FindPath(num, Goal);
        }

        var result = NextMovs.Last!.Value;

        NextMovs.RemoveLast();
        ExploredPos.Add(result.Item2);
        return result.Item1;
    }

    private (int, int) SelectEnd(IMaze maze)
    {
        Random random = new();

        while (true)
        {
            (int x, int y) NewPos = (random.Next(0, maze.Width), random.Next(0, maze.Length));

            if (!maze.Cells[NewPos.y, NewPos.x].IsWall && !ExploredPos.Contains(NewPos))
            {
                return NewPos;
            }
        }
    }
}

public class EnemyIA : EntityData
{
    LinkedList<(GameKey, (int x, int y))> NextMovs;
    IAStrategies strategies;

    public EnemyIA()
    {
        strategies = new();
        NextMovs = new();
    }

    public GameKey Movement(DataPlayer[] players, IMaze maze)
    {
        var nearbyPlayer = NearbyPlayer(players);
        (int goalX, int goalY) Goal;
        int[,] num;

        Goal = nearbyPlayer?.CurrentPosition ?? SelectEnd(maze);

        if (NextMovs.Count < 1 || nearbyPlayer?.CurrentPosition != NextMovs.Last?.Value.Item2)
        {
            num = strategies.Lee(maze, CurrentPosition, Goal);
            NextMovs = strategies.FindPath(num, Goal);
        }

        var result = NextMovs.Last!.Value;
        NextMovs.RemoveLast();
        return result.Item1;
    }

    private (int, int) SelectEnd(IMaze maze)
    {
        Random random = new Random();

        while (true)
        {
            (int x, int y) NewPos = (random.Next(maze.Width), random.Next(maze.Length));

            if (maze.Cells[NewPos.y, NewPos.x].IsWall || CurrentPosition == NewPos)
                continue;
            return NewPos;
        }
    }

    private DataPlayer? NearbyPlayer(DataPlayer[] players)
    {
        DataPlayer? nearbyPlayer = null;

        double minDistance = double.MaxValue;

        foreach (var player in players.Where(player => player.Visible))
        {
            if (CurrentPosition == player.CurrentPosition)
                continue;
            double distance = GeneralMethods.Distance(CurrentPosition, player.CurrentPosition);
            if (distance > minDistance)
                continue;

            minDistance = distance;
            nearbyPlayer = player;
        }

        return nearbyPlayer;
    }
}
