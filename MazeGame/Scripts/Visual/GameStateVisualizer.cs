using Spectre.Console;

public class GameStateVisualizer
{
    private int oldSize = 0;

    public void ShowInfo(DataPlayer player)
    {
        Console.SetCursorPosition(0, 0);
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn(new TableColumn("[bold yellow]Game Status[/]").Centered());

        table.AddRow(
            new Markup(
                $"[bold yellow]Se encuentra jugando el player:[/] [bold green]{player.Name}[/]"
            )
        );
        table.AddRow(new Markup("[bold yellow]Efectos negativos:[/]"));
        foreach (State state in player.States)
        {
            if (state.Activate)
            {
                table.AddRow(
                    new Markup($"[bold red]{state.GetType().Name}[/] - Duración: {state.Duration}")
                );
            }
        }
        table.AddRow(
            new Markup(
                $"[bold yellow]Skill Asignada:[/] [bold green]{player.AsignedSkill.GetType().Name[5..]}[/]"
            )
        );
        table.AddRow(
            new Markup($"[bold yellow]Skill Cooldown:[/] {player.AsignedSkill.CurrentCooldown}")
        );
        table.AddRow(new Markup($"[bold yellow]Movimientos restantes:[/] {player.CurrentActions}"));

        if (table.Rows.Count < oldSize)
        {
            int dif = oldSize - table.Rows.Count;
            for (int i = 0; i++ < dif; table.AddEmptyRow()) { }
        }

        oldSize = table.Rows.Count;
        AnsiConsole.Write(new Padder(table).Padding(30, 0, 0, 0));
    }
}
