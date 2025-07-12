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

    /// <summary>
    /// Одна стрелка перпендикулярно вверх
    /// </summary>
    public static TileParams OneArrowUp() => new(TileType.Arrow, ArrowsCodesHelper.OneArrowUp);
    
    /// <summary>
    /// Одна стрелка по диагонали правый верхний угол
    /// </summary>
    public static TileParams OneArrowDiagonal() => new(TileType.Arrow, ArrowsCodesHelper.OneArrowDiagonal);
    
    /// <summary>
    /// Две стрелки по диагонали правый верхний - левый нижний угол
    /// </summary>
    public static TileParams TwoArrowsDiagonal() => new(TileType.Arrow, ArrowsCodesHelper.TwoArrowsDiagonal);
    
    /// <summary>
    /// Две стрелки горизонтально на левую и правую стороны
    /// </summary>
    public static TileParams TwoArrowsLeftRight() => new(TileType.Arrow, ArrowsCodesHelper.TwoArrowsLeftRight);
    
    /// <summary>
    /// Три стрелки одна по диагонали левый верхний угол, две перпендикулярно право и низ
    /// </summary>
    public static TileParams ThreeArrows() => new(TileType.Arrow, ArrowsCodesHelper.ThreeArrows);
    
    /// <summary>
    /// Четыре стрелки перпендикулярно на все стороны
    /// </summary>
    public static TileParams FourArrowsPerpendicular() => new(TileType.Arrow, ArrowsCodesHelper.FourArrowsPerpendicular);
    
    /// <summary>
    /// Четыре стрелки по диагонали на все углы
    /// </summary>
    public static TileParams FourArrowsDiagonal() => new(TileType.Arrow, ArrowsCodesHelper.FourArrowsDiagonal);
}