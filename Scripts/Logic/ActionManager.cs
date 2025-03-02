public class ActionManager
{
    public void Options(EntityData entity, DataPlayer[] players, IMaze maze, GameKey key)
    {
        if (entity is DataPlayer player)
        {
            if (
                key == GameKey.Up
                || key == GameKey.Down
                || key == GameKey.Right
                || key == GameKey.Left
            )
            {
                Movement(player, maze, key);
            }

            if (key == GameKey.Space)
            {
                ExecuteSkill(players, player, maze);
            }
        }

        if (entity is EnemyIA enemyIA)
        {
            Movement(enemyIA, maze, key);
        }
    }

    private void ExecuteSkill(DataPlayer[] players, DataPlayer player, IMaze maze)
    {
        player.AsignedSkill.Execute(players, player, maze);
    }

    private void Movement(EntityData entity, IMaze maze, GameKey key)
    {
        int x = entity.CurrentPosition.x;
        int y = entity.CurrentPosition.y;

        int newY = y;
        int newX = x;

        switch (key)
        {
            case GameKey.Up:
                if (!IsValid(x, y - 1, maze))
                    break;
                newY--;
                break;

            case GameKey.Down:
                if (!IsValid(x, y + 1, maze))
                    break;
                newY++;
                break;

            case GameKey.Left:
                if (!IsValid(x - 1, y, maze))
                    break;
                newX--;
                break;

            case GameKey.Right:
                if (!IsValid(x + 1, y, maze))
                    break;
                newX++;
                break;
        }

        if (newX != x || newY != y)
        {
            ClearOldPosition(x, y, entity is EnemyIA, maze);
            x = newX;
            y = newY;
            entity.CurrentPosition = (x, y);
            UpdateNewPosition(x, y, entity is EnemyIA, maze);
        }
    }

    private bool IsValid(int x, int y, IMaze maze)
    {
        return x >= 0 && x < maze.Width && y >= 0 && y < maze.Length && !maze[y, x].IsWall;
    }

    private void UpdateNewPosition(int x, int y, bool isEnemy, IMaze maze)
    {
        if (isEnemy)
        {
            maze[y, x].IsEnemy = true;
            return;
        }
        maze[y, x].IsCharacter = true;
    }

    private void ClearOldPosition(int x, int y, bool isEnemy, IMaze maze)
    {
        if (isEnemy)
        {
            maze[y, x].IsEnemy = false;
            return;
        }
        maze[y, x].IsCharacter = false;
    }
}
