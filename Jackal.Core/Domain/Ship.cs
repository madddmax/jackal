using Newtonsoft.Json;

namespace Jackal.Core.Domain;

public record Ship
{
    [JsonProperty]
    public int TeamId;

    [JsonProperty]
    public Position Position;

    public Ship(int teamId, Position position)
    {
        TeamId = teamId;
        Position = position;
    }
}