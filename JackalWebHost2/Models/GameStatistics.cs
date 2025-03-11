namespace JackalWebHost2.Models;

/// <summary>
/// Текущая статистика игры
/// </summary>
public class GameStatistics
{
    public List<DrawTeam> Teams;
    public int TurnNo;
    public bool IsGameOver;
    public string GameMessage;
    public int CurrentTeamId;
}