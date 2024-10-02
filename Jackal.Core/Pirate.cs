using System;
using Newtonsoft.Json;

namespace Jackal.Core;

public class Pirate(int teamId, TilePosition position, PirateType type)
{
    [JsonProperty]
    public readonly Guid Id = Guid.NewGuid();

    [JsonProperty]
    public PirateType Type = type;

    [JsonProperty]
    public readonly int TeamId = teamId;

    [JsonProperty]
    public TilePosition Position = position;

    /// <summary>
    /// Напился на бочке с ромом
    /// </summary>
    [JsonProperty]
    public bool IsDrunk;
        
    /// <summary>
    /// Номер хода с которого пират поддался пьянству,
    /// наша команда категорически не одобряет пьянство
    /// </summary>
    internal int? DrunkSinceTurnNo;

    /// <summary>
    /// Попал в ловушку
    /// </summary>
    [JsonProperty]
    public bool IsInTrap;
    
    /// <summary>
    /// Застрял в дыре
    /// </summary>
    [JsonProperty]
    public bool IsInHole;
        
    // TODO-MAD Оптимизация DrawService - заполнять при изменении позиции и сбрасывать каждый ход
    public bool Changed;

    /// <summary>
    /// Пират доступен для хода
    /// </summary>
    public bool IsActive => IsDrunk == false && IsInTrap == false && IsInHole == false;
    
    /// <summary>
    /// Сбросить все эффекты недоступности
    /// </summary>
    public void ResetEffects()
    {
        IsInTrap = false;
        IsInHole = false;
        IsDrunk = false;
        DrunkSinceTurnNo = null;
    }
}