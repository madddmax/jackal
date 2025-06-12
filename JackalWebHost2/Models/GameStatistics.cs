namespace JackalWebHost2.Models;

/// <summary>
/// Текущая статистика игры
/// </summary>
public class GameStatistics
{
    public int TurnNumber;
    public bool IsGameOver;
    public string GameMessage;
    public int CurrentTeamId;
}