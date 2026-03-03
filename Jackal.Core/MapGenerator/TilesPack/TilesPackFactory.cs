using System.Collections.Generic;

namespace Jackal.Core.MapGenerator.TilesPack;

public static class TilesPackFactory
{
    public const string Extended = "extended";
    public const string Basic = "basic";
    public const string Difficult = "difficult";
    public const string AllGold = "all-gold";
    
    public static string CheckName(string? name) =>
        name switch
        {
            Basic => Basic,
            Difficult => Difficult,
            AllGold => AllGold,
            _ => Extended
        };
    
    public static ITilesPack Create(string? name) =>
        name switch
        {
            Basic => new BasicTilesPack(),
            Difficult => new DifficultTilesPack(),
            AllGold => new AllGoldTilesPack(),
            _ => new ExtendedTilesPack()
        };
    
    public static List<string> GetAll() => [Extended, Basic, Difficult];
}