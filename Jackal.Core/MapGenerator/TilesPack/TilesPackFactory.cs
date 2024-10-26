namespace Jackal.Core.MapGenerator.TilesPack;

public static class TilesPackFactory
{
    public static ITilesPack Create(string? name) =>
        name switch
        {
            "Classic" => new ClassicTilesPack(),
            _ => new FinamTilesPack()
        };
}