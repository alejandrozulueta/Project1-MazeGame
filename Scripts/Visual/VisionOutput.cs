using System.Text;

public class VisionOutput
{
    StringBuilder sb = new();

    public void PrintPlayerVision(DataPlayer player, DataPlayer[] players, IMaze maze)
    {
        char[,] currentState = new char[maze.Length, maze.Width];
        sb.Clear();

        for (int i = 0; i < maze.Length; i++)
        {
            for (int j = 0; j < maze.Width; j++)
            {
                currentState[i, j] = ' ';
            }
        }

        foreach (var Coord in player.Vision.ActualVision)
        {
            int x = Coord.x;
            int y = Coord.y;
            char symbol = ' ';

            if (maze[y, x].IsWall)
            {
                symbol = '█';
            }
            else if (maze[y, x].IsGoal)
            {
                symbol = 'M';
            }
            else if (maze[y, x].IsEnemy)
            {
                symbol = 'H';
            }
            else if (maze[y, x].IsCharacter)
            {
                foreach (
                    var visiblePlayer in players.Where(p =>
                        p.CurrentPosition == (x, y) && (p == player || p.Visible)
                    )
                )
                {
                    symbol = visiblePlayer.Name;
                }
            }

            currentState[y, x] = symbol;
        }

        Console.SetCursorPosition(0, 0);

        for (int i = 0; i < maze.Length; i++)
        {
            for (int j = 0; j < maze.Width; j++)
            {
                sb.Append(currentState[i, j]);
            }

            sb.AppendLine();
        }

        Console.Write(sb);
    }
}
