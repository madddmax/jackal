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
    
    /// <summary>
    /// Бутылка с ромом
    /// </summary>
    /// <param name="count">Количество бутылок</param>
    public static TileParams RumBottle(int count = 1) => new(TileType.RumBottle, count);    

    /// <summary>
    /// Лес - требуется 2 хода для прохождения клетки
    /// </summary>
    public static TileParams SpinningForest() => new(TileType.Spinning) { SpinningCount = 2 };
    
    /// <summary>
    /// Пустыня - требуется 3 хода для прохождения клетки
    /// </summary>
    public static TileParams SpinningDesert() => new(TileType.Spinning) { SpinningCount = 3 };
    
    /// <summary>
    /// Болото - требуется 4 хода для прохождения клетки
    /// </summary>
    public static TileParams SpinningSwamp() => new(TileType.Spinning) { SpinningCount = 4 };
    
    /// <summary>
    /// Гора - требуется 5 ходов для прохождения клетки
    /// </summary>
    public static TileParams SpinningMount() => new(TileType.Spinning) { SpinningCount = 5 };
}