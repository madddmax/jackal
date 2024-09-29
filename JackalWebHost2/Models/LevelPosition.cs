using Jackal.Core;

namespace JackalWebHost2.Models;

public class LevelPosition(TilePosition position)
{
    public int X = position.X;
    
    public int Y = position.Y;
    
    public int Level = position.Level;
}