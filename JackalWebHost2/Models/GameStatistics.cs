using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JackalWebHost.Models;

/// <summary>
/// Текущая статистика игры
/// </summary>
public class GameStatistics
{
    public List<DrawTeam> Teams;
    public int TurnNo;
    public bool IsGameOver;
    public int CurrentTeamId;
    public bool IsHumanPlayer;
}