using Spectre.Console;

public class MenuManager
{
    public Menu<Type> MenuSkills;
    public Menu<int> MenuPlayers;
    public Menu<int> MenuIAs;
    public Menu<OptionsPlayerConstructor> MenuPlayerConstructor;
    public Menu<GameDifficulty> MenuSetGameDifficult;

    public MenuManager()
    {
        MenuSkills = new(MenuList.MenuSkills);
        MenuPlayers = new(MenuList.MenuPlayer);
        MenuIAs = new(MenuList.MenuIA);
        MenuPlayerConstructor = new(MenuList.MenuPlayerConstructor);
        MenuSetGameDifficult = new(MenuList.MenuSetGameDifficult);
    }
}

public class Menu<T>
{
    public List<MenuItems<T>> Data { get; private set; }

    public Menu(List<MenuItems<T>> data)
    {
        Data = data;
    }

    public T Activate()
    {
        var item = SelectOption();
        return item;
    }

    private T SelectOption()
    {
        var options = Data.Select(d => d.Text).ToArray();
        var selectedOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Seleccione una opción:").AddChoices(options)
        );

        return Data.First(d => d.Text == selectedOption).Value!;
    }
}

public class MenuItems<T>
{
    public T Value { get; private set; }
    public (int x, int y) Position { get; private set; }
    public string Text { get; private set; }

    public MenuItems(T value, (int, int) position, string text)
    {
        Value = value;
        Position = position;
        Text = text;
    }
}

public static class MenuList
{
    public static List<MenuItems<Type>> MenuSkills =
    [
        new(typeof(SkillShowMap), (20, 5), "Mostrar Mapa"),
        new(typeof(SkillChangePosition), (20, 6), "Intercambiar Posiciones"),
        new(typeof(SkillShowGoal), (20, 7), "Mostrar Meta"),
        new(typeof(SkillCreateTemporalWall), (20, 8), "Pared Temporal"),
        new(typeof(SkillShowTramps), (20, 9), "Mostrar Trampas"),
        new(typeof(SkillTurnBreaker), (20, 10), "Quitar turno"),
        new(typeof(SkillInvisibility), (20, 11), "Invisibility"),
    ];

    public static List<MenuItems<int>> MenuPlayer =
    [
        new(1, (20, 5), "1 Jugador/es"),
        new(2, (20, 6), "2 Jugador/es"),
        new(3, (20, 7), "3 Jugador/es"),
        new(4, (20, 8), "4 Jugador/es"),
    ];

    public static List<MenuItems<int>> MenuIA =
    [
        new(0, (20, 5), "0 IA/s"),
        new(1, (20, 6), "1 IA/s"),
        new(2, (20, 7), "2 IA/s"),
        new(3, (20, 8), "3 IA/s"),
        new(4, (20, 9), "4 IA/s"),
    ];

    public static List<MenuItems<OptionsPlayerConstructor>> MenuPlayerConstructor =
    [
        new(OptionsPlayerConstructor.SetSkill, (20, 5), "Elegir habilidad"),
        new(OptionsPlayerConstructor.SetActions, (20, 6), "Elegir número de acciones"),
        new(OptionsPlayerConstructor.SetRangeOfVision, (20, 7), "Elegir rango de visión"),
    ];

    public static List<MenuItems<GameDifficulty>> MenuSetGameDifficult =
    [
        new(GameDifficulty.Easy, (20, 5), "Fácil"),
        new(GameDifficulty.Medium, (20, 5), "Medio"),
        new(GameDifficulty.Hard, (20, 5), "Difícil"),
    ];
}
