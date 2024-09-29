using Newtonsoft.Json;

namespace Jackal.Core;

public class Map(int mapSize)
{
    [JsonProperty] 
    private readonly Tile[,] Tiles = new Tile[mapSize, mapSize];

    public Tile this[Position pos]
    {
        get { return Tiles[pos.X, pos.Y]; }
        internal set { Tiles[pos.X, pos.Y] = value; }
    }

    public TileLevel this[TilePosition pos]
    {
        get { return Tiles[pos.Position.X, pos.Position.Y].Levels[pos.Level]; }
    }

    public Tile this[int x, int y]
    {
        get { return Tiles[x, y]; }
        internal set { Tiles[x, y] = value; }
    }
}