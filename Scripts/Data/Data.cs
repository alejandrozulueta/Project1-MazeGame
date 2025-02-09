public enum GameKey
{
    Up,
    Down,
    Right,
    Left,
    Space,
    Esc,
}

public enum OptionsPlayerConstructor
{
    SetRangeOfVision,
    SetActions,
    SetSkill,
}

public enum GameDifficulty
{
    Easy,
    Medium,
    Hard,
}

public interface IMaze
{
    int Length { get; }
    int Width { get; }
    (int, int) Scape { get; }
    Cell[,] Cells { get; set; }
}

public class Cell
{
    public bool IsWall { get; set; }
    public bool IsGoal { get; set; }
    public bool IsCharacter { get; set; }
    public bool IsTramp { get; set; }
    public bool IsEnemy { get; set; }
}

public abstract class EntityData
{
    public char Name;
    public int AssignedNum;
    public (int x, int y) CurrentPosition;
    public int TotalActions;
    public int CurrentActions;
}

public class DataPlayer : EntityData
{
    public required int RangeOfVision;
    public required List<(int x, int y)> Vision;
    public required List<State> States;
    public required Skill AsignedSkill;
    public bool Visible = true;
    public bool Turn = true;
}
