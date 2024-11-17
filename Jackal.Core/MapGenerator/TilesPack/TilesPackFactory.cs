using System.Collections.Generic;

namespace Jackal.Core.MapGenerator.TilesPack;

public static class TilesPackFactory
{
    private const string Extended = "extended";
    private const string Classic = "classic";
    private const string Madddmax = "madddmax";
    private const string AllGold = "all-gold";
    private const string LotsOfBen = "lots-of-ben";
    
    public static string CheckName(string? name) =>
        name switch
        {
            Classic => Classic,
            Madddmax => Madddmax,
            AllGold => AllGold,
            LotsOfBen => LotsOfBen,
            _ => Extended
        };
    
    public static ITilesPack Create(string? name) =>
        name switch
        {
            Classic => new ClassicTilesPack(),
            Madddmax => new MadddmaxTilesPack(),
            AllGold => new AllGoldTilesPack(),
            LotsOfBen => new LotsOfBenTilesPack(),
            _ => new ExtendedTilesPack()
        };
    
    public static List<string> GetAll() => [Extended, Classic, Madddmax, AllGold, LotsOfBen];
}