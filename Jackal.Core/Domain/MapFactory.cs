namespace Jackal.Core.Domain;

public static class MapFactory
{
    public static Map Create(int mapSize, Team[] teams)
    {
        var map = new Map(mapSize);

        for (int i = 0; i < mapSize; i++)
        {
            map.SetWater(i, 0);
            map.SetWater(0, i);
            map.SetWater(i, mapSize - 1);
            map.SetWater(mapSize - 1, i);
        }

        for (int x = 1; x < mapSize - 1; x++)
        {
            for (int y = 1; y < mapSize - 1; y++)
            {
                if ((x == 1 || x == mapSize - 2) && (y == 1 || y == mapSize - 2))
                    map.SetWater(x, y);
                else
                    map.SetUnknown(x, y);
            }
        }

        map.SetPiratesOnMap(teams);
        
        return map;
    }

    private static void SetWater(this Map map, int x, int y)
    {
        var tile = new Tile(TileParams.Water(x, y));
        map[x, y] = tile;
    }

    private static void SetUnknown(this Map map, int x, int y)
    {
        var tile = new Tile(TileParams.Unknown(x, y));
        map[x, y] = tile;
    }
    
    private static void SetPiratesOnMap(this Map map, Team[] teams)
    {
        foreach (var team in teams)
        {
            foreach (var pirate in team.Pirates)
            {
                map[team.ShipPosition].Pirates.Add(pirate);
            }
        }
    }
}