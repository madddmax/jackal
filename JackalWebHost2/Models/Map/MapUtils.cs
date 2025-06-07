using Jackal.Core.Domain;

namespace JackalWebHost2.Models.Map;

public static class MapUtils
{
    public static MapPositionId ToMapPositionId(Position shipPosition, int mapSize)
    {
        if (shipPosition.Y == 0)
            return MapPositionId.Down;

        if (shipPosition.X == 0)
            return MapPositionId.Left;

        if (shipPosition.Y == mapSize - 1)
            return MapPositionId.Up;

        if (shipPosition.X == mapSize - 1)
            return MapPositionId.Right;

        throw new ArgumentException("Wrong init ship position", nameof(shipPosition));
    }
}