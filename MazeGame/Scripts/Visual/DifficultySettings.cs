public class DifficultySettings
{
    public (int x, int y) MazeDimensions { get; set; }
    public int TrampCount { get; set; }
    public int EnemyCount { get; set; }

    public DifficultySettings(GameDifficulty difficulty)
    {
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                MazeDimensions = (10, 10);
                TrampCount = 0;
                EnemyCount = 0;
                break;

            case GameDifficulty.Medium:
                MazeDimensions = (20, 20);
                TrampCount = 10;
                EnemyCount = 1;
                break;

            case GameDifficulty.Hard:
                MazeDimensions = (30, 30);
                TrampCount = 20;
                EnemyCount = 2;
                break;
        }
    }
}
