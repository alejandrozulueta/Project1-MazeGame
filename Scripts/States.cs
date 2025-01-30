public abstract class State
{
    public bool Activate = false;
    public int TotalDuration = 3;
    public int Duration = 0;

    public virtual bool ApplyEffect(DataPlayer dataPlayer, IMaze maze, GameKey key = GameKey.Esc)
    {
        if (Activate && Duration == 0)
            Duration = TotalDuration;
        return Activate;
    }

    public virtual bool RestoreEffect(DataPlayer player)
    {
        if (!Activate)
            return false;
        if (--Duration <= 0)
        {
            Activate = false;
            Duration = 0;
        }
        return !Activate;
    }
}

public class StateBlinded : State
{
    private List<(int, int)> OldVision;

    public StateBlinded()
    {
        OldVision = [];
    }

    public override bool ApplyEffect(DataPlayer dataPlayer, IMaze maze, GameKey key)
    {
        var activate = base.ApplyEffect(dataPlayer, maze, key);
        if (activate)
            Blind(dataPlayer, maze);
        return activate;
    }

    protected virtual void Blind(DataPlayer player, IMaze maze)
    {
        OldVision = [.. OldVision.Union(player.Vision)];
        player.Vision.Clear();

        int startX = player.CurrentPosition.x;
        int startY = player.CurrentPosition.y;
        int range = player.RangeOfVision / 2;

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = -range; dy <= range; dy++)
            {
                int x = startX + dx;
                int y = startY + dy;

                if (x >= 0 && x < maze.Width && y >= 0 && y < maze.Length)
                {
                    if (player.Vision.Contains((x, y)))
                        continue;

                    player.Vision.Add((x, y));
                }
            }
        }
    }

    protected virtual void RestoreVision(DataPlayer player)
    {
        player.Vision = [.. player.Vision.Union(OldVision)];
    }

    public override bool RestoreEffect(DataPlayer player)
    {
        var restore = base.RestoreEffect(player);
        if (restore)
            RestoreVision(player);
        return restore;
    }
}

public class StateDisorted : State
{
    public new GameKey ApplyEffect(DataPlayer dataPlayer, IMaze maze, GameKey option)
    {
        var activate = base.ApplyEffect(dataPlayer, maze);
        if (activate)
            return Disoriented(option);
        return option;
    }

    protected virtual GameKey Disoriented(GameKey option)
    {
        return option switch
        {
            GameKey.Up => GameKey.Down,
            GameKey.Down => GameKey.Up,
            GameKey.Right => GameKey.Left,
            GameKey.Left => GameKey.Right,
            GameKey.Space => GameKey.Space,
            _ => GameKey.Esc,
        };
    }
}

public class StateDeath : State
{
    public override bool ApplyEffect(DataPlayer player, IMaze maze, GameKey key)
    {
        var activate = base.ApplyEffect(player, maze, key);
        if (activate)
            Death(player, maze);
        return activate;
    }

    protected virtual void Death(DataPlayer player, IMaze maze)
    {
        player.Turn = false;
        player.Visible = false;

        maze.Cells[player.CurrentPosition.y, player.CurrentPosition.x].IsCharacter = false;

        (int newX, int newY) = DeathPos(maze);
        player.CurrentPosition = (newX, newY);
        maze.Cells[newY, newX].IsCharacter = true;
    }

    private (int, int) DeathPos(IMaze maze)
    {
        for (int i = 1; i < maze.Length; i++)
        {
            for (int j = 1; j < maze.Width; j++)
            {
                if (maze.Cells[i, j].IsWall)
                    continue;
                if (maze.Cells[i, j].IsCharacter)
                    continue;
                if (maze.Cells[i, j].IsEnemy)
                    continue;

                return (j, i);
            }
        }

        return (1, 1);
    }

    public override bool RestoreEffect(DataPlayer player)
    {
        var restore = base.RestoreEffect(player);
        if (restore)
            Revive(player);
        return restore;
    }

    protected virtual void Revive(DataPlayer player)
    {
        player.Turn = true;
        player.Visible = true;
    }
}
