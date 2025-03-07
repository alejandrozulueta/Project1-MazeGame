using Newtonsoft.Json;

public class GameManager
{
    public int TotalTurns { get; set; }
    public EntityManager EntityManager { get; set; }
    public ActionManager ActionManager { get; set; }
    public TrampsManager TrampsManager { get; set; }
    public VisionOutput VisionOutput { get; set; }
    public GameStateVisualizer GameStateVisualizer;
    public Events Events;
    public Maze Maze { get; set; }
    public string DefaultSavePath { get; set; }

    public GameManager(DifficultySettings settings, (int, int) countPlayers, string defaultSavePath)
    {
        TotalTurns = 0;
        Maze = new Maze(settings.MazeDimensions);
        VisionOutput = new();
        GameStateVisualizer = new();
        TrampsManager = new TrampsManager(settings.TrampCount, Maze);
        EntityManager = new EntityManager(countPlayers, settings.EnemyCount, Maze);
        ActionManager = new ActionManager();
        Events = new();
        DefaultSavePath = defaultSavePath;
    }

    public GameManager(GameState gameState, string defaultSavePath)
    {
        EntityManager = new EntityManager(gameState.DataEntities, gameState.DataPlayers);
        ActionManager = new();
        TrampsManager = new TrampsManager(gameState.DataTramps, gameState.CountTramps);
        VisionOutput = new();
        GameStateVisualizer = new();
        Events = new();
        Maze = new Maze(gameState.Length, gameState.Width, gameState.Cells, gameState.Scape);
        DefaultSavePath = defaultSavePath;
    }

    public void SaveGame(string savePath)
    {
        var saveGame = new GameState()
        {
            TotalTurns = TotalTurns,
            CountTramps = TrampsManager.CountTramps,
            DataEntities = EntityManager.DataEntities,
            DataPlayers = EntityManager.DataPlayers,
            DataTramps = TrampsManager.DataTramps,
            Length = Maze.Length,
            Width = Maze.Width,
            Cells = Maze.Cells,
            Scape = Maze.Scape,
        };

        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        string json = JsonConvert.SerializeObject(saveGame, Formatting.Indented, settings);
        File.WriteAllText(savePath, json);
    }

    public void ProcessTurns()
    {
        Console.Clear();
        while (true)
        {
            TotalTurns++;
            SaveGame(DefaultSavePath);

            TrampsManager.RestoreAllTrampsCooldown();

            foreach (var entity in EntityManager.DataEntities)
            {
                if (!NextTurn(entity))
                {
                    Events.Victory((DataPlayer)entity);
                    Thread.Sleep(2000);
                    return;
                }
            }
        }
    }

    private bool NextTurn(EntityData entity)
    {
        bool move = entity switch
        {
            DataPlayer player => MovePlayer(player),
            EnemyIA enemyIA => MoveEnemy(enemyIA),
            _ => throw new NotImplementedException(),
        };
        entity.CurrentActions = entity.TotalActions;
        return move;
    }

    private bool MoveEnemy(EnemyIA enemyIA)
    {
        while (enemyIA.CurrentActions > 0)
        {
            var option = SelecKey(enemyIA);
            ActionManager.Options(enemyIA, EntityManager.DataPlayers, Maze, option);
            enemyIA.CurrentActions--;

            var killedPlayer = KillPlayer(enemyIA);
            if (killedPlayer is not null)
            {
                Events.KillEffect(killedPlayer);
            }
        }
        return true;
    }

    private bool MovePlayer(DataPlayer player)
    {
        player.AsignedSkill.RestoreEffects(EntityManager.DataPlayers, Maze);
        player.States.ForEach(state => state.RestoreEffect(player));

        while (player.CurrentActions > 0 && player.Turn)
        {
            player.Vision.UpdateVision(player.CurrentPosition, Maze);
            GameStateVisualizer.ShowInfo(player);
            VisionOutput.PrintPlayerVision(player, EntityManager.DataPlayers, Maze);

            var option = SelecKey(player);

            if (player.States[1] is StateDisorted state)
            {
                option = state.ApplyEffect(player, Maze, option);
            }

            ActionManager.Options(player, EntityManager.DataPlayers, Maze, option);
            player.CurrentActions--;

            var killedPlayer = KillPlayer(player);
            if (killedPlayer is not null)
            {
                Events.KillEffect(killedPlayer);
            }

            var tramp = TrampsManager.CheckTrampActivation(player);
            tramp?.ActivateTramp(player, Maze);

            if (tramp is not null)
            {
                Events.TrampEffect(player, tramp);
            }

            player
                .States.Where(state => state is not StateDisorted)
                .ToList()
                .ForEach(state => state.ApplyEffect(player, Maze));

            if (Victory(player))
            {
                return false;
            }
        }
        return true;
    }

    private bool Victory(DataPlayer player)
    {
        return Maze.Scape == player.CurrentPosition;
    }

    private GameKey SelecKey(EntityData entity)
    {
        if (entity is PlayerIA inst)
        {
            Thread.Sleep(250);
            return inst.Movement(Maze);
        }
        else if (entity is EnemyIA enemyInst)
        {
            return enemyInst.Movement(EntityManager.DataPlayers, Maze);
        }
        else
        {
            return GetInfo.GetKey();
        }
    }

    private DataPlayer? KillPlayer(EntityData entity)
    {
        foreach (
            var entities in EntityManager.DataEntities.Where(inst =>
                inst is not EnemyIA && inst != entity
            )
        )
        {
            if (entities is DataPlayer player)
            {
                if (
                    entity.CurrentPosition != entities.CurrentPosition
                    || player.States[2].Activate == true
                )
                    continue;
                player.States[2].Activate = true;
                player.States[2].ApplyEffect(player, Maze);
                return player;
            }
        }

        return null;
    }
}
