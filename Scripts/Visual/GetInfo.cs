public static class GetInfo
{
    public static int GetValue()
    {
        return int.Parse(Console.ReadLine());
    }

    public static GameKey GetKey()
    {
        ConsoleKey key = Console.ReadKey().Key;

        return key switch
        {
            ConsoleKey.UpArrow => GameKey.Up,
            ConsoleKey.DownArrow => GameKey.Down,
            ConsoleKey.LeftArrow => GameKey.Left,
            ConsoleKey.RightArrow => GameKey.Right,
            ConsoleKey.Spacebar => GameKey.Space,
            _ => GameKey.Esc,
        };
    }
}
