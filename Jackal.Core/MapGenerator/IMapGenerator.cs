namespace Jackal.Core;

public interface IMapGenerator
{
    public int MapId { get; }
    
    int CoinsOnMap { get; }

    Tile GetNext(Position position);
}