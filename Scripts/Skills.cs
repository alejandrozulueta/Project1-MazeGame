using System.Text;

public abstract class Skill
{
    public int TotalCooldown { get; protected set; }
    public int CurrentCooldown { get; protected set; }

    public Skill(int totalCooldown)
    {
        TotalCooldown = totalCooldown;
    }

    public void Execute(DataPlayer[] players, DataPlayer player, IMaze maze)
    {
        if (CurrentCooldown > 0)
            return;

        Action(players, player, maze);

        CurrentCooldown = TotalCooldown;
    }

    public void RestoreEffects(DataPlayer[] players, IMaze maze)
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown--;
            return;
        }

        Restore(players, maze);
    }

    protected virtual void Action(DataPlayer[] players, DataPlayer player, IMaze maze) { }

    protected virtual void Restore(DataPlayer[] dataPlayers, IMaze maze) { }
}

public class SkillShowMap : Skill
{
    public SkillShowMap(int totalCooldown)
        : base(totalCooldown) { }

    protected override void Action(DataPlayer[] players, DataPlayer player, IMaze maze) =>
        ShowMap(maze);

    private void ShowMap(IMaze maze)
    {
        StringBuilder sb = new();

        for (int y = 0; y < maze.Length; y++)
        {
            for (int x = 0; x < maze.Width; x++)
            {
                if (maze[y, x].IsWall)
                {
                    sb.Append('█');
                }
                else
                {
                    sb.Append(' ');
                }
            }

            sb.AppendLine();
        }

        Console.SetCursorPosition(0, 0);
        Console.Write(sb);
        Thread.Sleep(3000);
    }
}

public class SkillChangePosition : Skill
{
    public SkillChangePosition(int totalCooldown)
        : base(totalCooldown) { }

    protected override void Action(DataPlayer[] players, DataPlayer player, IMaze maze) =>
        ChangePosition(players, player);

    private void ChangePosition(DataPlayer[] players, DataPlayer player)
    {
        var newPlayer = GetInfo.GetPlayer(players);

        (int, int) OldPosition = player.CurrentPosition;

        player.CurrentPosition = newPlayer.CurrentPosition;
        newPlayer.CurrentPosition = OldPosition;
    }
}

public class SkillShowGoal : Skill
{
    public SkillShowGoal(int totalCooldown)
        : base(totalCooldown) { }

    protected override void Action(DataPlayer[] players, DataPlayer player, IMaze maze) =>
        ShowGoal(maze);

    private void ShowGoal(IMaze maze)
    {
        Console.SetCursorPosition(maze.Scape.Item1, maze.Scape.Item2);
        Console.Write('G');
        Thread.Sleep(1500);
    }
}

public class SkillCreateTemporalWall : Skill
{
    int effectCooldown;
    (int, int) modificateWall;
    bool isMod;

    public SkillCreateTemporalWall(int totalCooldown)
        : base(totalCooldown) { }

    protected override void Action(DataPlayer[] players, DataPlayer player, IMaze maze) =>
        CreateTemporalWall(player, maze);

    private void CreateTemporalWall(DataPlayer player, IMaze maze)
    {
        effectCooldown = 3;
        CurrentCooldown = TotalCooldown;

        (int x, int y) = player.CurrentPosition;

        switch (GetInfo.GetKey())
        {
            case GameKey.Up:
                if (!maze[y - 1, x].IsEmpty)
                    return;
                isMod = true;
                maze[y - 1, x].IsWall = true;
                modificateWall = (y - 1, x);
                break;

            case GameKey.Down:
                if (!maze[y + 1, x].IsEmpty)
                    return;
                isMod = true;
                maze[y + 1, x].IsWall = true;
                modificateWall = (y + 1, x);
                break;

            case GameKey.Right:
                if (!maze[y, x + 1].IsEmpty)
                    return;
                isMod = true;
                maze[y, x + 1].IsWall = true;
                modificateWall = (y, x + 1);
                break;

            case GameKey.Left:
                if (!maze[y, x - 1].IsEmpty)
                    return;
                isMod = true;
                maze[y, x - 1].IsWall = true;
                modificateWall = (y, x - 1);
                break;
        }
    }

    protected override void Restore(DataPlayer[] dataPlayers, IMaze maze)
    {
        if (effectCooldown-- > 0 || !isMod)
            return;

        maze[modificateWall.Item1, modificateWall.Item2].IsWall = isMod = false;
    }
}

public class SkillShowTramps : Skill
{
    public SkillShowTramps(int totalCooldown)
        : base(totalCooldown) { }

    protected override void Action(DataPlayer[] players, DataPlayer player, IMaze maze) =>
        ShowTramps(player, maze);

    public void ShowTramps(DataPlayer player, IMaze maze)
    {
        foreach (var (x, y) in player.Vision.ActualVision)
        {
            if (!maze[y, x].IsTramp)
                continue;

            Console.SetCursorPosition(x, y);
            Console.Write("T");
        }

        Thread.Sleep(500);
    }
}

public class SkillTurnBreaker : Skill
{
    DataPlayer? player;

    public SkillTurnBreaker(int totalCooldown)
        : base(totalCooldown) { }

    protected override void Action(DataPlayer[] players, DataPlayer player, IMaze maze) =>
        TurnBreaker(players);

    public void TurnBreaker(DataPlayer[] players)
    {
        player = GetInfo.GetPlayer(players);
        player.Turn = false;
    }

    protected override void Restore(DataPlayer[] players, IMaze maze)
    {
        if (player == null || player.Turn)
            return;

        player.Turn = true;
    }
}

public class SkillInvisibility : Skill
{
    int EffectDuration;
    DataPlayer? player;

    public SkillInvisibility(int totalCooldown)
        : base(totalCooldown) { }

    protected override void Action(DataPlayer[] players, DataPlayer player, IMaze maze) =>
        Invisibility(player);

    private void Invisibility(DataPlayer player)
    {
        EffectDuration += 2;
        this.player = player;
        player.Visible = false;
    }

    protected override void Restore(DataPlayer[] dataPlayers, IMaze maze)
    {
        if (player == null)
            return;

        if (EffectDuration == 0)
        {
            player!.Visible = true;
            player = null;
        }
        else
        {
            EffectDuration--;
        }
    }
}
