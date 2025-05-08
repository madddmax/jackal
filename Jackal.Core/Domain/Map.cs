using Newtonsoft.Json;

namespace Jackal.Core.Domain;

public class Map(int mapSize)
{
    [JsonProperty] 
    private readonly Tile[,] Tiles = new Tile[mapSize, mapSize];

    public Tile this[Position pos]
    {
        get => Tiles[pos.X, pos.Y];
        internal set => Tiles[pos.X, pos.Y] = value;
    }

    public TileLevel this[TilePosition pos] => 
        Tiles[pos.Position.X, pos.Position.Y].Levels[pos.Level];

    public Tile this[int x, int y]
    {
        get => Tiles[x, y];
        internal set => Tiles[x, y] = value;
    }
}