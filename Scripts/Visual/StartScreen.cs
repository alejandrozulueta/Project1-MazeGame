using Newtonsoft.Json;
using Spectre.Console;

public class StartScreen
{
    public void Start()
    {
        string currentPath = Directory.GetCurrentDirectory();
        string savePath = Path.Combine(currentPath, "Saves");
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        savePath = Path.Combine(savePath, "saveGame.json");
        while (true)
        {
            Console.Clear();
            Console.CursorVisible = false;

            MenuManager menu = new MenuManager();

            var panelContent = new Panel(
                new Rows(
                    new FigletText("... Enchanted Maze").Centered().Color(Color.Green),
                    new Rule("[bold yellow]¡Bienvenido a ... Enchanted Maze![/]")
                        .RuleStyle("yellow")
                        .Centered(),
                    new Rule("[italic]Una aventura poco emocionante te espera...[/]")
                        .RuleStyle("green")
                        .Centered()
                )
            )
                .Border(BoxBorder.Double)
                .BorderColor(Color.Blue)
                .Header("[bold yellow]Pantalla de Inicio[/]", Justify.Center)
                .Expand();

            AnsiConsole.Write(panelContent);

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .AddChoices(new[] { "Iniciar Juego", "Cargar Partida", "Salir" })
            );

            switch (option)
            {
                case "Iniciar Juego":
                    AnsiConsole.MarkupLine("[bold green]Iniciando el juego...[/]");
                    AnsiConsole.Write(new FigletText("¡A Jugar!").Centered().Color(Color.Green));
                    Thread.Sleep(2000);
                    GameManager gameManager = new(
                        new DifficultySettings(menu.MenuSetGameDifficult.Activate()),
                        (menu.MenuPlayers.Activate(), menu.MenuIAs.Activate()),
                        savePath
                    );
                    gameManager.ProcessTurns();
                    break;

                case "Cargar Partida":
                    GameState? savedGame = LoadGameState(savePath);
                    if (savedGame == null)
                        break;
                    AnsiConsole.MarkupLine("[bold green]Iniciando el juego...[/]");
                    AnsiConsole.Write(new FigletText("¡A Jugar!").Centered().Color(Color.Green));
                    Thread.Sleep(3000);
                    gameManager = new GameManager(savedGame, savePath);
                    gameManager.ProcessTurns();
                    break;

                case "Salir":
                    AnsiConsole.MarkupLine("[bold red]Saliendo del juego...[/]");
                    return;
            }
        }
    }

    private GameState? LoadGameState(string savePath)
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            GameState? savedGame = JsonConvert.DeserializeObject<GameState>(json, settings);
            if (savedGame != null)
                return savedGame;
        }
        AnsiConsole.MarkupLine("[bold red]Error de carga[/]");
        AnsiConsole.MarkupLine("[bold]Volviendo a la pantalla de inicio...[/]");
        Thread.Sleep(1000);
        return null;
    }
}
