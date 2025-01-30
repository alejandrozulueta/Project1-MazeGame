public static class GeneralMethods
{
    public static double Distance((int x, int y) pos1, (int x, int y) pos2)
    {
        return Math.Sqrt(Math.Pow(pos1.x - pos2.x, 2) + Math.Pow(pos1.y - pos2.y, 2));
    }
}
