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
    int range;

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
        OldVision = [.. OldVision.Union(player.Vision.ActualVision)];
        player.Vision.ActualVision.Clear();

        if (range == 0)
        {
            range = player.Vision.rangeOfVision;
            player.Vision.rangeOfVision = range / 2;
        }
    }

    protected virtual void RestoreVision(DataPlayer player)
    {
        player.Vision.ActualVision = [.. player.Vision.ActualVision.Union(OldVision)];
        player.Vision.rangeOfVision = range;
        range = 0;
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

        maze[player.CurrentPosition.y, player.CurrentPosition.x].IsCharacter = false;

        (int newX, int newY) = DeathPos(maze);
        player.CurrentPosition = (newX, newY);
        maze[newY, newX].IsCharacter = true;
    }

    private (int, int) DeathPos(IMaze maze)
    {
        for (int i = 1; i < maze.Length; i++)
        {
            for (int j = 1; j < maze.Width; j++)
            {
                if (maze[i, j].IsWall)
                    continue;
                if (maze[i, j].IsCharacter)
                    continue;
                if (maze[i, j].IsEnemy)
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
