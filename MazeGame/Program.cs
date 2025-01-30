namespace Project
{
    public static class Program
    {
        public static void Main()
        {
            Console.CursorVisible = false;
            StartScreen startScreen = new StartScreen();
            startScreen.Start();
        }
    }
}
