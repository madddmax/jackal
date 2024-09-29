namespace Jackal.Core.MapGenerator;

public interface IMapGenerator
{
    public int MapId { get; }
    
    int CoinsOnMap { get; }

    Tile GetNext(Position position);
}