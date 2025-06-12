using System;
using Newtonsoft.Json;

namespace Jackal.Core.Domain;

public record Pirate(int TeamId, TilePosition Position, PirateType Type)
{
    [JsonProperty]
    public readonly Guid Id = Guid.NewGuid();

    [JsonProperty]
    public PirateType Type = Type;

    [JsonProperty]
    public readonly int TeamId = TeamId;

    [JsonProperty]
    public TilePosition Position = Position;

    /// <summary>
    /// Напился на бочке с ромом
    /// </summary>
    [JsonProperty]
    public bool IsDrunk;
        
    /// <summary>
    /// Номер хода с которого пират поддался пьянству,
    /// наша команда категорически не одобряет пьянство
    /// </summary>
    public int? DrunkSinceTurnNumber;

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

    /// <summary>
    /// Пират доступен для хода
    /// </summary>
    public bool IsActive => IsDrunk == false && IsInTrap == false && IsInHole == false;
    
    /// <summary>
    /// Пират застрял
    /// </summary>
    public bool IsDisable => IsInTrap || IsInHole;

    /// <summary>
    /// Сбросить все эффекты недоступности
    /// </summary>
    public void ResetEffects()
    {
        IsInTrap = false;
        IsInHole = false;
        IsDrunk = false;
        DrunkSinceTurnNumber = null;
    }
    
    public virtual bool Equals(Pirate? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id) && 
               Position.Equals(other.Position) && 
               IsDrunk == other.IsDrunk && 
               IsInTrap == other.IsInTrap && 
               IsInHole == other.IsInHole;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}