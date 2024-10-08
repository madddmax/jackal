﻿namespace JackalWebHost2.Models;

public class GameSettings
{
    /// <summary>
    /// Игроки robot/human
    /// </summary>
    public string[] Players { get; set; } = null!;
        
    /// <summary>
    /// ИД карты, по нему генерируется расположение клеток
    /// </summary>
    public int? MapId { get; set; }
        
    /// <summary>
    /// Размер стороны карты с учетом воды
    /// </summary>
    public int? MapSize { get; set; }
}