using Newtonsoft.Json;
using Spectre.Console;

public class StartScreen
{
    public void Start()
    {
        string targetFolder = "MazeGame";
        string pathToMazeGame = FileManager.FindPathToFolder(targetFolder);
        string savePath = Path.Combine(pathToMazeGame, "Saves", "saveGame.json");

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

        AnsiConsole
            .Progress()
            .Start(ctx =>
            {
                var task = ctx.AddTask("[green]Cargando...[/]");
                while (!ctx.IsFinished)
                {
                    task.Increment(1.5);
                    Thread.Sleep(100);
                }
            });

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
                    (menu.MenuPlayers.Activate(), menu.MenuIAs.Activate())
                );
                gameManager.ProcessTurns();
                break;

            case "Cargar Partida":
                if (!File.Exists(savePath))
                    return;
                string json = File.ReadAllText(savePath);
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                };

                GameState savedGame = JsonConvert.DeserializeObject<GameState>(json, settings);

                AnsiConsole.MarkupLine("[bold green]Iniciando el juego...[/]");
                AnsiConsole.Write(new FigletText("¡A Jugar!").Centered().Color(Color.Green));
                Thread.Sleep(3000);
                gameManager = new GameManager(savedGame);
                gameManager.ProcessTurns();
                break;

            case "Salir":
                AnsiConsole.MarkupLine("[bold red]Saliendo del juego...[/]");
                return;
        }
    }
}
