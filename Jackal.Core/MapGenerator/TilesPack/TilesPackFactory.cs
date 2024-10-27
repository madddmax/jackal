using System.Collections.Generic;

namespace Jackal.Core.MapGenerator.TilesPack;

public static class TilesPackFactory
{
    private const string Extended = "extended";
    private const string Classic = "classic";
    
    public static ITilesPack Create(string? name) =>
        name switch
        {
            Classic => new ClassicTilesPack(),
            _ => new ExtendedTilesPack()
        };

    public static List<string> GetAll() => [Extended, Classic];
}