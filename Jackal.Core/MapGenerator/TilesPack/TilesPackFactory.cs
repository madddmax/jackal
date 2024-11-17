using System.Collections.Generic;

namespace Jackal.Core.MapGenerator.TilesPack;

public static class TilesPackFactory
{
    private const string Extended = "extended";
    private const string Classic = "classic";
    private const string Difficult = "difficult";
    private const string AllGold = "all-gold";
    
    public static string CheckName(string? name) =>
        name switch
        {
            Classic => Classic,
            Difficult => Difficult,
            AllGold => AllGold,
            _ => Extended
        };
    
    public static ITilesPack Create(string? name) =>
        name switch
        {
            Classic => new ClassicTilesPack(),
            Difficult => new DifficultTilesPack(),
            AllGold => new AllGoldTilesPack(),
            _ => new ExtendedTilesPack()
        };
    
    public static List<string> GetAll() => [Extended, Classic, Difficult, AllGold];
}