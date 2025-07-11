namespace Jackal.Core.Domain;

public static class TileFactory
{
    /// <summary>
    /// Монета
    /// </summary>
    /// <param name="count">Количество монет</param>
    public static TileParams Coin(int count = 1) => new(TileType.Coin, count);
    
    /// <summary>
    /// Большая монета
    /// </summary>
    /// <param name="count">Количество больших монет</param>
    public static TileParams BigCoin(int count = 1) => new(TileType.BigCoin, count);
}