public class TrampsManager
{
    public Tramp[] DataTramps { get; private set; }
    public int CountTramps { get; private set; }

    public TrampsManager(Tramp[] dataTramps, int countTramps)
    {
        DataTramps = dataTramps;
        CountTramps = countTramps;
    }

    public TrampsManager(int countTramps, IMaze maze)
    {
        CountTramps = countTramps;
        DataTramps = new Tramp[countTramps];

        for (int i = 0; i < countTramps; i++)
        {
            DataTramps[i] = RandomTramp(maze);
            (int x, int y) = DataTramps[i].Position;
            maze[y, x].IsTramp = true;
        }
    }

    private Tramp RandomTramp(IMaze maze)
    {
        int n = Randoms.random.Next(3);

        return n switch
        {
            0 => new TrampBlind(Randoms.RandomPosition(maze)),
            1 => new TrampTeleport(Randoms.RandomPosition(maze)),
            2 => new TrampDisort(Randoms.RandomPosition(maze)),
            _ => throw new InvalidOperationException(""),
        };
    }

    public Tramp? CheckTrampActivation(DataPlayer player)
    {
        foreach (
            var tramp in DataTramps.Where(p =>
                p.Position == player.CurrentPosition && p.Cooldown == 0
            )
        )
        {
            return tramp;
        }

        return null;
    }

    public void RestoreAllTrampsCooldown()
    {
        foreach (var tramp in DataTramps)
        {
            tramp.RestoreTrampCooldown();
        }
    }
}
