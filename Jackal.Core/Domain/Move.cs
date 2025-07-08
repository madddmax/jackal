using System;
using Newtonsoft.Json;

namespace Jackal.Core.Domain;

/// <summary>
/// Ход
/// </summary>
[method: JsonConstructor]
public record Move(TilePosition From, TilePosition To, Position? Prev, MoveType Type = MoveType.Usual)
{
    /// <summary>
    /// Откуда идем
    /// </summary>
    [JsonProperty] 
    public readonly TilePosition From = From ?? throw new ArgumentException(nameof(From));
        
    /// <summary>
    /// Куда идем
    /// </summary>
    [JsonProperty] 
    public readonly TilePosition To = To ?? throw new ArgumentException(nameof(To));

    /// <summary>
    /// Предыдущая позиция
    /// </summary>
    [JsonProperty]
    public readonly Position? Prev = Prev;
    
    /// <summary>
    /// Тип хода
    /// </summary>
    [JsonProperty]
    public readonly MoveType Type = Type;

    /// <summary>
    /// Использовать бутылку с ромом
    /// </summary>
    public bool WithRumBottle =>
        Type is MoveType.WithRumBottle or MoveType.WithRumBottleAndCoin or MoveType.WithRumBottleAndBigCoin;
    
    /// <summary>
    /// Перенос монеты
    /// </summary>
    public bool WithCoin => Type is MoveType.WithCoin or MoveType.WithRumBottleAndCoin;

    /// <summary>
    /// Перенос большой монеты
    /// </summary>
    public bool WithBigCoin => Type is MoveType.WithBigCoin or MoveType.WithRumBottleAndBigCoin;
    
    /// <summary>
    /// Воскрешение пирата на бабе
    /// </summary>
    public bool WithRespawn => Type == MoveType.WithRespawn;

    /// <summary>
    /// Открытие неизвестной клетки с маяка
    /// </summary>
    public bool WithLighthouse => Type == MoveType.WithLighthouse;

    /// <summary>
    /// Выбор клетки для разлома
    /// </summary>
    public bool WithQuake => Type == MoveType.WithQuake;
}