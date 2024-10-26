using System.Collections.Generic;

namespace Jackal.Core.MapGenerator.TilesPack;

public static class TilesPackFactory
{
    private const string Finam = "finam";
    private const string Classic = "classic";
    
    public static ITilesPack Create(string? name) =>
        name switch
        {
            Classic => new ClassicTilesPack(),
            _ => new FinamTilesPack()
        };

    public static List<string> GetAll() => [Finam, Classic];
}