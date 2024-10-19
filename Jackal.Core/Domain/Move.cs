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
    /// Перенос монеты
    /// </summary>
    public bool WithCoin => Type == MoveType.WithCoin;

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