using Spectre.Console;

public class Events
{
    public void Victory(DataPlayer player)
    {
        Console.Clear();
        var panel = new Panel($"[bold yellow]¡Felicidades {player.Name} ha ganado![/]")
        {
            Border = BoxBorder.Double,
            Padding = new Padding(1, 1, 1, 1),
            Header = new PanelHeader("[bold green]Victoria[/]"),
        };
        for (int i = 0; i < 4; i++)
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup(
                $"[bold white on red]¡Felicidades jugador {player.Name} ha ganado![/]"
            );
            Thread.Sleep(250);

            AnsiConsole.Clear();
            AnsiConsole.Markup(
                $"[bold white on green]¡Felicidades jugador {player.Name} ha ganado![/]"
            );
            Thread.Sleep(250);

            AnsiConsole.Clear();
            AnsiConsole.Markup(
                $"[bold white on blue]¡Felicidades jugador {player.Name} ha ganado![/]"
            );
            Thread.Sleep(250);

            AnsiConsole.Clear();
            AnsiConsole.Markup(
                $"[bold white on yellow]¡Felicidades jugador {player.Name} ha ganado![/]"
            );
            Thread.Sleep(250);
        }

        AnsiConsole.Clear();
        AnsiConsole.Render(panel);
    }

    public void TrampEffect(DataPlayer player, Tramp tramp)
    {
        Console.Clear();
        var panel = new Panel(
            $"[bold red]El jugador {player.Name} ha caído en una {tramp.GetType().Name}[/]"
        )
        {
            Border = BoxBorder.Double,
            Padding = new Padding(1, 1, 1, 1),
            Header = new PanelHeader("[bold yellow]¡Atención![/]"),
        };

        AnsiConsole
            .Live(panel)
            .Start(ctx =>
            {
                AnsiConsole.Write(panel);
            });

        Thread.Sleep(2000);
        Console.Clear();
    }

    public void KillEffect(DataPlayer killedPlayer)
    {
        Console.Clear();
        var panel = new Panel($"[bold red]El jugador {killedPlayer.Name} ha sido asesinado[/]")
        {
            Border = BoxBorder.Double,
            Padding = new Padding(1, 1, 1, 1),
            Header = new PanelHeader("[bold yellow]¡Atención![/]"),
        };

        AnsiConsole
            .Live(panel)
            .Start(ctx =>
            {
                AnsiConsole.Write(panel);
            });

        Thread.Sleep(2000);
        Console.Clear();
    }
}
