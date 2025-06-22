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
    /// Воскрешение пирата на бабе
    /// </summary>
    WithRespawn = 2,
        
    /// <summary>
    /// Открытие неизвестной клетки с маяка
    /// </summary>
    WithLighthouse = 3,
    
    /// <summary>
    /// Выбор клетки для разлома
    /// </summary>
    WithQuake = 4,
    
    /// <summary>
    /// Перенос большой монеты
    /// </summary>
    WithBigCoin = 5,
    
    /// <summary>
    /// Выход за бутылку с ромом
    /// </summary>
    WithRumBottle = 6,
    
    /// <summary>
    /// Выход за бутылку с ромом с монетой
    /// </summary>
    WithRumBottleAndCoin = 7,
    
    /// <summary>
    /// Выход за бутылку с ромом с большой монетой
    /// </summary>
    WithRumBottleAndBigCoin = 8,
};