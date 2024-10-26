using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

public class ClassicTilesPack : ITilesPack
{
    public string Name => "Classic";

    public TileParams[] AllTiles { get; } =
    [

    ];
}