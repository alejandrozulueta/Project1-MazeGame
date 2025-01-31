using Spectre.Console;

public static class GetInfo
{
    public static DataPlayer GetPlayer(DataPlayer[] players)
    {
        Console.Clear();
        var playerNames = players.Select(p => p.Name).ToArray();

        char selectedPlayerName = AnsiConsole.Prompt(
            new SelectionPrompt<char>()
                .Title("[yellow]Seleccione un jugador:[/]")
                .PageSize(10)
                .HighlightStyle(new Style(foreground: Color.Blue))
                .AddChoices(playerNames)
        );

        var selectedPlayer = players.First(p => p.Name == selectedPlayerName);
        return selectedPlayer;
    }

    public static GameKey GetKey()
    {
        ConsoleKey key = Console.ReadKey(false).Key;

        return key switch
        {
            ConsoleKey.UpArrow or ConsoleKey.W => GameKey.Up,
            ConsoleKey.DownArrow or ConsoleKey.S => GameKey.Down,
            ConsoleKey.LeftArrow or ConsoleKey.A => GameKey.Left,
            ConsoleKey.RightArrow or ConsoleKey.D => GameKey.Right,
            ConsoleKey.Spacebar => GameKey.Space,
            _ => GameKey.Esc,
        };
    }
}
