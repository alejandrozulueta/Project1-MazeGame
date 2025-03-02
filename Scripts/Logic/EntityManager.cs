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
                AsignedSkill = dataPlayer.AsignedSkill!,
                TotalActions = dataPlayer.Actions,
                CurrentActions = dataPlayer.Actions,
                Vision = new(range, newPos, maze),
                States = [new StateBlinded(), new StateDisorted(), new StateDeath()],
            };

            if (DataEntities[i] is DataPlayer player)
            {
                DataPlayers[i] = player;
            }

            maze[newPos.y, newPos.x].IsCharacter = true;
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
                Vision = new(range, newPos, maze),
                States = [new StateBlinded(), new StateDisorted(), new StateDeath()],
            };

            if (DataEntities[i] is DataPlayer player)
            {
                DataPlayers[i] = player;
            }

            maze[newPos.y, newPos.x].IsCharacter = true;
        }

        for (int i = num.players + num.playersIA; i < DataEntities.Length; i++)
        {
            var newPos = Randoms.RandomPosition(maze);
            var minPlayerMovement = DataPlayers.Select(x => x.TotalActions).Min();

            DataEntities[i] = new EnemyIA()
            {
                TotalActions = minPlayerMovement,
                CurrentActions = minPlayerMovement,
                CurrentPosition = newPos,
            };

            maze[newPos.y, newPos.x].IsEnemy = true;
        }
    }

    private char SetChar()
    {
        return chars.Dequeue();
    }
}
