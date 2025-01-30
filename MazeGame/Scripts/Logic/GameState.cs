public class GameState
{
    public required int TotalTurns { get; set; }
    public required int CountTramps { get; set; }
    public required EntityData[] DataEntities { get; set; }
    public required DataPlayer[] DataPlayers { get; set; }
    public required Tramp[] DataTramps { get; set; }

    public required int Length { get; set; }
    public required int Width { get; set; }
    public required Cell[,] Cells { get; set; }
    public required (int, int) Scape { get; set; }
}
