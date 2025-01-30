public abstract class Tramp
{
    public (int, int) Position { get; protected set; }
    public int Cooldown { get; protected set; }

    public Tramp((int, int) position)
    {
        Position = position;
    }

    public void ActivateTramp(DataPlayer player, IMaze maze)
    {
        if (!(Cooldown == 0))
            return;
        TrampEffect(player, maze);
        Cooldown += 3;
    }

    public void RestoreTrampCooldown()
    {
        if (Cooldown == 0)
            return;
        Cooldown--;
    }

    public abstract void TrampEffect(DataPlayer player, IMaze maze);
}

public class TrampBlind : Tramp
{
    public TrampBlind((int, int) position)
        : base(position) { }

    public override void TrampEffect(DataPlayer player, IMaze maze) => Blind(player);

    private void Blind(DataPlayer player)
    {
        player.States[0].Activate = true;
    }
}

public class TrampDisort : Tramp
{
    public TrampDisort((int, int) position)
        : base(position) { }

    public override void TrampEffect(DataPlayer player, IMaze maze) => Disort(player);

    private void Disort(DataPlayer player)
    {
        player.States[1].Activate = true;
    }
}

public class TrampTeleport : Tramp
{
    public TrampTeleport((int, int) position)
        : base(position) { }

    public override void TrampEffect(DataPlayer player, IMaze maze) => Teleport(player, maze);

    private void Teleport(DataPlayer player, IMaze maze)
    {
        maze.Cells[player.CurrentPosition.y, player.CurrentPosition.x].IsCharacter = false;
        player.CurrentPosition = Randoms.RandomPosition(maze);
        maze.Cells[player.CurrentPosition.y, player.CurrentPosition.x].IsCharacter = true;
    }
}
