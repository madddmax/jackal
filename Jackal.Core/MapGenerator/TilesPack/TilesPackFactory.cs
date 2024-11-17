using System.Collections.Generic;

namespace Jackal.Core.MapGenerator.TilesPack;

public static class TilesPackFactory
{
    private const string Extended = "extended";
    private const string Classic = "classic";
    private const string Madddmax = "madddmax";
    private const string AllGold = "all-gold";
    
    public static string CheckName(string? name) =>
        name switch
        {
            Classic => Classic,
            Madddmax => Madddmax,
            AllGold => AllGold,
            _ => Extended
        };
    
    public static ITilesPack Create(string? name) =>
        name switch
        {
            Classic => new ClassicTilesPack(),
            Madddmax => new MadddmaxTilesPack(),
            AllGold => new AllGoldTilesPack(),
            _ => new ExtendedTilesPack()
        };
    
    public static List<string> GetAll() => [Extended, Classic, Madddmax, AllGold];
}