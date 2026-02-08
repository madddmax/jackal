using Jackal.Core.Domain;

namespace JackalWebHost2.Models;

public class TileChange
{
    /// <summary>
    /// Тип клетки
    /// </summary>
    public string TileType;
    
    public int Rotate;
    
    public bool IsUnknown;

    public LevelChange[] Levels;

    public int X;
    public int Y;
}