using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Jackal.Core.Domain;

public record TileLevel(TilePosition Position)
{
    [JsonProperty]
    public int Coins;

    [JsonProperty]
    public readonly TilePosition Position = Position;

    [JsonProperty]
    public readonly HashSet<Pirate> Pirates = [];

    [JsonIgnore]
    public int? OccupationTeamId => Pirates.Count > 0 ? Pirates.First().TeamId : null;

    public bool HasNoEnemy(int[] enemyTeamIds) => 
        OccupationTeamId.HasValue == false || !enemyTeamIds.Contains(OccupationTeamId.Value);
    
    public virtual bool Equals(TileLevel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Coins == other.Coins && 
               Position.Equals(other.Position) && 
               Pirates.SequenceEqual(other.Pirates);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Pirates);
    }
}