public class IAStrategies
{
    public LinkedList<(GameKey, (int x, int y))> FindPath(int[,] num, (int, int) goal)
    {
        LinkedList<(GameKey, (int x, int y))> Path = new();

        (int currentX, int currentY) = goal;
        int contador = num[currentY, currentX];

        List<(GameKey, (int x, int y))> ValidMovs = new List<(GameKey, (int x, int y))>();

        Random random = new();

        while (contador != 1)
        {
            contador--;
            ValidMovs.Clear();

            foreach (var dir in Moves)
            {
                int newX = currentX - dir.Item2.x;
                int newY = currentY - dir.Item2.y;

                if (!CompPosition(newX, newY, num))
                    continue;
                if (!(num[newY, newX] == contador))
                    continue;

                ValidMovs.Add((dir.Item1, (newX, newY)));
            }

            (GameKey, (int x, int y)) selection;

            try
            {
                selection = ValidMovs[random.Next(ValidMovs.Count)];
            }
            catch (ArgumentOutOfRangeException)
            {
                Path.AddLast((GameKey.Esc, (currentX, currentY)));
                return Path;
            }

            Path.AddLast((selection.Item1, (currentX, currentY)));
            (currentX, currentY) = selection.Item2;
        }

        return Path;
    }

    public int[,] Lee(IMaze maze, (int, int) currentPosition, (int scapeX, int scapeY) goal)
    {
        (int X, int Y) = currentPosition;
        int[,] Num = new int[maze.Length, maze.Width];

        int Contador = 1;
        Num[Y, X] = Contador;

        bool isChange = true;
        bool findGoal = false;

        while (isChange && !findGoal)
        {
            Contador++;
            isChange = false;

            for (int k = 0; k < maze.Length; k++)
            {
                for (int j = 0; j < maze.Width; j++)
                {
                    if (Num[k, j] != Contador - 1)
                        continue;
                    foreach (var i in Moves.Select(item => item.Item2))
                    {
                        (int ChangeX, int ChangeY) = i;
                        int NewY = k + ChangeY;
                        int NewX = j + ChangeX;

                        if (!CompPosition(NewX, NewY, Num))
                            continue;
                        if (maze.Cells[NewY, NewX].IsWall)
                            continue;
                        if (Num[NewY, NewX] <= Contador && Num[NewY, NewX] > 0)
                            continue;

                        Num[NewY, NewX] = Contador;
                        isChange = true;
                        findGoal |= (NewX, NewY) == goal;
                    }
                }
            }
        }

        return Num;
    }

    public bool CompPosition(int x, int y, int[,] array)
    {
        return x >= 0 && x < array.GetLength(1) && y >= 0 && y < array.GetLength(0);
    }

    public List<(GameKey, (int x, int y))> Moves =
    [
        (GameKey.Down, (0, 1)),
        (GameKey.Up, (0, -1)),
        (GameKey.Right, (1, 0)),
        (GameKey.Left, (-1, 0)),
    ];
}
