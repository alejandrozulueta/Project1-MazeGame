using Spectre.Console;

public class PlayerConstructor
{
    private MenuManager menu = new MenuManager();

    private int Value = 0;

    public int Actions = 1;
    public int RangeOfVision = 1;
    public Skill AsignedSkill;

    private bool exit = false;

    public PlayerConstructor()
    {
        Menu();
    }

    private void Menu()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("CREACIÓN DE PERSONAJE").Centered().Color(Color.Green));
        Thread.Sleep(2000);

        while (!exit)
        {
            SelectOption();
        }

        Console.Clear();
    }

    private void SelectOption()
    {
        ShowCurrentInformation();
        var menuOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione una opción:")
                .AddChoices(
                    "Asignar Habilidad",
                    "Ajustar Acciones",
                    "Ajustar Rango de Visión",
                    "Salir"
                )
        );

        switch (menuOption)
        {
            case "Asignar Habilidad":
                SetSkill();
                break;

            case "Ajustar Acciones":
                SetActions();
                break;

            case "Ajustar Rango de Visión":
                SetRangeOfVision();
                break;

            case "Salir":
                exit = true;
                return;
        }
    }

    private void SetActions()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("AJUSTAR ACCIONES").Centered().Color(Color.Yellow));

        while (true)
        {
            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.RightArrow && Value <= 95)
            {
                Actions++;
                Value += 5;
            }
            else if (key == ConsoleKey.LeftArrow && Value > 0 && Actions > 1)
            {
                Actions--;
                Value -= 5;
            }
            else if (key == ConsoleKey.Enter)
            {
                break;
            }

            ShowCurrentInformation();
        }
    }

    private void SetRangeOfVision()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("AJUSTAR RANGO DE VISIÓN").Centered().Color(Color.Blue));

        while (true)
        {
            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.RightArrow && Value <= 95)
            {
                RangeOfVision++;
                Value += 5;
            }
            else if (key == ConsoleKey.LeftArrow && Value > 0 && RangeOfVision > 1)
            {
                RangeOfVision--;
                Value -= 5;
            }
            else if (key == ConsoleKey.Enter)
            {
                break;
            }

            ShowCurrentInformation();
        }
    }

    private void SetSkill()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("ASIGNAR HABILIDAD").Centered().Color(Color.BlueViolet));

        var type = menu.MenuSkills.Activate();

        AsignedSkill = type.Name switch
        {
            "SkillShowMap" => new SkillShowMap(2),
            "SkillChangePosition" => new SkillChangePosition(3),
            "SkillShowGoal" => new SkillShowGoal(2),
            "SkillCreateTemporalWall" => new SkillCreateTemporalWall(2),
            "SkillShowTramps" => new SkillShowTramps(2),
            "SkillTurnBreaker" => new SkillTurnBreaker(2),
            "SkillInvisibility" => new SkillInvisibility(2),
            _ => throw new NotImplementedException(),
        };

        ShowCurrentInformation();
    }

    private void ShowCurrentInformation()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Panel(
                new Markup(
                    $"[bold]Acciones:[/] {Actions}\n[bold]Valor:[/] {Value}\n[bold]Habilidad Asignada:[/] {AsignedSkill?.GetType().Name ?? "Ninguna"}\n[bold]Rango de Visión:[/] {RangeOfVision}"
                )
            )
                .Header("INFORMACIÓN ACTUAL")
                .BorderColor(Color.Cyan1)
                .Expand()
        );
    }
}
