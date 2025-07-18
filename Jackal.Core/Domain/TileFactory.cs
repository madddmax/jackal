using System;

namespace Jackal.Core.Domain;

public static class TileFactory
{
    /// <summary>
    /// Пустая клетка
    /// </summary>
    /// <param name="imageNumber">Номер изображения</param>
    public static TileParams Empty(int imageNumber = 1)
    {
        if (imageNumber < 1 || imageNumber > 4)
            throw new ArgumentException(
                "Номер изображения для TileType.Empty должен быть от 1 до 4 включительно",
                nameof(imageNumber)
            );
        
        return new TileParams(TileType.Empty, imageNumber);
    }

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

    /// <summary>
    /// Дыра, задаём направление клетки чтобы пираты не закрывали изображение дыры
    /// </summary>
    public static TileParams Hole() => new(TileType.Hole) { Direction = DirectionType.Left };
    
    /// <summary>
    /// Форт
    /// </summary>
    public static TileParams Fort() => new(TileType.Fort);
    
    /// <summary>
    /// Воскрешающий форт
    /// </summary>
    public static TileParams RespawnFort() => new(TileType.RespawnFort);
    
    /// <summary>
    /// Людоед
    /// </summary>
    public static TileParams Cannibal() => new(TileType.Cannibal);
    
    /// <summary>
    /// Бочка с ромом
    /// </summary>
    public static TileParams RumBarrel() => new(TileType.RumBarrel);
    
    /// <summary>
    /// Лошадь
    /// </summary>
    public static TileParams Horse() => new(TileType.Horse);
    
    /// <summary>
    /// Воздушный шар
    /// </summary>
    public static TileParams Balloon() => new(TileType.Balloon);
    
    /// <summary>
    /// Самолёт
    /// </summary>
    public static TileParams Airplane() => new(TileType.Airplane);
    
    /// <summary>
    /// Крокодил
    /// </summary>
    public static TileParams Crocodile() => new(TileType.Crocodile);
    
    /// <summary>
    /// Лёд
    /// </summary>
    public static TileParams Ice() => new(TileType.Ice);
}