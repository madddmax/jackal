using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jackal.Core;

public class TileLevel
{
    [JsonProperty]
    public int Coins;

    [JsonProperty]
    public readonly TilePosition Position;

    [JsonProperty]
    public readonly HashSet<Pirate> Pirates = new HashSet<Pirate>();

    [JsonIgnore]
    public int? OccupationTeamId
    {
        get
        {
            if (Pirates != null)
            {
                foreach (Pirate pirate in Pirates)
                {
                    return pirate.TeamId;
                }
            }
            return null;
        }
    }

    public TileLevel(TilePosition position)
    {
        Position = position;
    }
    
    public bool HasNoEnemy(int ownTeamId) => OccupationTeamId.HasValue == false || OccupationTeamId == ownTeamId;
}