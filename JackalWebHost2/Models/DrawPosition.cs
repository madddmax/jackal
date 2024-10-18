using Jackal.Core.Domain;

namespace JackalWebHost2.Models;

public class DrawPosition(Position position)
{
    public int X = position.X;
    
    public int Y = position.Y;
}