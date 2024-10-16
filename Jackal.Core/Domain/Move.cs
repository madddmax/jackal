using System;
using Newtonsoft.Json;

namespace Jackal.Core.Domain;

[method: JsonConstructor]
public record Move(TilePosition From, TilePosition To, MoveType Type = MoveType.Usual)
{
    [JsonProperty] 
    public readonly TilePosition From = From ?? throw new ArgumentException(nameof(From));
        
    [JsonProperty] 
    public readonly TilePosition To = To ?? throw new ArgumentException(nameof(To));
    
    [JsonProperty]
    public readonly MoveType Type = Type;

    /// <summary>
    /// Перенос монеты
    /// </summary>
    public bool WithCoins => Type == MoveType.WithCoin;

    /// <summary>
    /// Воскрешение пирата на бабе
    /// </summary>
    public bool WithRespawn => Type == MoveType.WithRespawn;

    /// <summary>
    /// Открытие неизвестной клетки с маяка
    /// </summary>
    public bool WithLighthouse => Type == MoveType.WithLighthouse;

    /// <summary>
    /// Замена клеток разломом
    /// </summary>
    public bool WithQuake => Type == MoveType.WithQuake;

    public Move(Position from, Position to, MoveType type = MoveType.Usual)
        : this(new TilePosition(from), new TilePosition(to), type)
    {
    }
}