using System;
using Newtonsoft.Json;

namespace Jackal.Core.Domain;

public record TilePosition
{
    [JsonProperty] 
    public readonly Position Position;

    [JsonIgnore]
    public int X => Position.X;

    [JsonIgnore]
    public int Y => Position.Y;

    [JsonProperty] 
    public readonly int Level;

    public TilePosition(int x, int y, int level = 0) 
        : this(new Position(x, y), level)
    {
    }
        
    [JsonConstructor]
    public TilePosition(Position position, int level = 0)
    {
        if (position == null) 
            throw new ArgumentNullException(nameof(position));
            
        if (level < 0 || level > 4) 
            throw new ArgumentException(nameof(level));
            
        Position = position;
        Level = level;
    }
}