public static class GetInfo
{
    public static int GetValue()
    {
        return int.Parse(Console.ReadLine());
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
