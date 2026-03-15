namespace Jackal.Core.Domain;

/// <summary>
/// Тип хода
/// </summary>
public enum MoveType
{
    /// <summary>
    /// Обычный
    /// </summary>
    Usual = 0,
        
    /// <summary>
    /// Перенос монеты
    /// </summary>
    WithCoin = 1,
       
    /// <summary>
    /// Перенос большой монеты
    /// </summary>
    WithBigCoin = 2,
    
    /// <summary>
    /// Выход за бутылку с ромом
    /// </summary>
    WithRumBottle = 3,
    
    /// <summary>
    /// Выход за бутылку с ромом с монетой
    /// </summary>
    WithRumBottleAndCoin = 4,
    
    /// <summary>
    /// Выход за бутылку с ромом с большой монетой
    /// </summary>
    WithRumBottleAndBigCoin = 5,
    
    /// <summary>
    /// Воскрешение пирата на бабе
    /// </summary>
    WithRespawn = 6,
        
    /// <summary>
    /// Открытие неизвестной клетки с маяка
    /// </summary>
    WithLighthouse = 7,
    
    /// <summary>
    /// Выбор первой клетки для разлома
    /// </summary>
    WithQuakeFirst = 8,
    
    /// <summary>
    /// Выбор второй клетки для разлома
    /// </summary>
    WithQuakeLast = 9,
};