public class EntityManager
{
    public EntityData[] DataEntities { get; private set; }
    public DataPlayer[] DataPlayers { get; private set; }

    private readonly Queue<char> chars = new Queue<char>(
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()
    );

    public EntityManager(EntityData[] dataEntities, DataPlayer[] dataPlayers)
    {
        DataEntities = dataEntities;
        DataPlayers = new DataPlayer[dataPlayers.Length];

        for (int i = 0; i < dataPlayers.Length; i++)
        {
            if (dataEntities[i] is DataPlayer player)
            {
                DataPlayers[i] = player;
            }
        }
    }

    public EntityManager((int players, int playersIA) num, int enemy, IMaze maze)
    {
        DataEntities = new EntityData[num.players + num.playersIA + enemy];
        DataPlayers = new DataPlayer[num.players + num.playersIA];

        for (int i = 0; i < num.players; i++)
        {
            var dataPlayer = new PlayerConstructor();
            var newPos = Randoms.RandomPosition(maze);
            int range = dataPlayer.RangeOfVision;

            DataEntities[i] = new DataPlayer
            {
                AssignedNum = i,
                CurrentPosition = newPos,
                Name = SetChar(),
                AsignedSkill = dataPlayer.AsignedSkill,
                TotalActions = dataPlayer.Actions,
                CurrentActions = dataPlayer.Actions,
                RangeOfVision = range,
                Vision = SetInitialVision(newPos, range, maze),
                States = new List<State>
                {
                    new StateBlinded(),
                    new StateDisorted(),
                    new StateDeath(),
                },
            };

            if (DataEntities[i] is DataPlayer player)
            {
                DataPlayers[i] = player;
            }

            maze.Cells[newPos.y, newPos.x].IsCharacter = true;
        }

        for (int i = num.players; i < num.players + num.playersIA; i++)
        {
            var newPos = Randoms.RandomPosition(maze);
            int range = 5;

            DataEntities[i] = new PlayerIA()
            {
                AssignedNum = i,
                CurrentPosition = newPos,
                Name = SetChar(),
                AsignedSkill = new SkillChangePosition(2),
                TotalActions = 5,
                CurrentActions = 5,
                RangeOfVision = range,
                Vision = SetInitialVision(newPos, range, maze),
                States = new List<State>
                {
                    new StateBlinded(),
                    new StateDisorted(),
                    new StateDeath(),
                },
            };

            if (DataEntities[i] is DataPlayer player)
            {
                DataPlayers[i] = player;
            }

            maze.Cells[newPos.y, newPos.x].IsCharacter = true;
        }

        for (int i = num.players + num.playersIA; i < DataEntities.Length; i++)
        {
            var newPos = Randoms.RandomPosition(maze);

            DataEntities[i] = new EnemyIA()
            {
                TotalActions = 3,
                CurrentActions = 3,
                CurrentPosition = newPos,
            };

            maze.Cells[newPos.y, newPos.x].IsEnemy = true;
        }
    }

    private char SetChar()
    {
        return chars.Dequeue();
    }

    private List<(int, int)> SetInitialVision((int x, int y) position, int visionRange, IMaze maze)
    {
        var vision = new List<(int, int)>();

        (int startX, int startY) = position;
        int range = visionRange;

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = -range; dy <= range; dy++)
            {
                int x = startX + dx;
                int y = startY + dy;

                if (x >= 0 && x < maze.Width && y >= 0 && y < maze.Length)
                {
                    vision.Add((x, y));
                }
            }
        }

        return vision;
    }
}
