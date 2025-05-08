using Newtonsoft.Json;

namespace Jackal.Core.Domain;

/// <summary>
/// Команда пиратов
/// </summary>
public record Team
{
    /// <summary>
    /// ИД команды
    /// </summary>
    public readonly int Id;
    
    /// <summary>
    /// Имя команды
    /// </summary>
    public readonly string Name;
    
    /// <summary>
    /// Позиция корабля
    /// </summary>
    public Position ShipPosition;
    
    /// <summary>
    /// Пираты - бравые ребята
    /// </summary>
    public Pirate[] Pirates;
    
    /// <summary>
    /// ИД команд противников
    /// </summary>
    public int[] EnemyTeamIds;

    /// <summary>
    /// ИД команды союзника
    /// </summary>
    public int? AllyTeamId;
    
    /// <summary>
    /// Монеты на корабле
    /// </summary>
    public int Coins;

    [JsonConstructor]
    public Team(int id, string name, Position shipPosition, Pirate[] pirates)
    {
        Id = id;
        Name = name;
        ShipPosition = shipPosition;
        Pirates = pirates;
    }
    
    public Team(int id, string name, int x, int y, int piratesPerPlayer)
    {
        Id = id;
        Name = name;
        ShipPosition = new Position(x, y);
        
        Pirates = new Pirate[piratesPerPlayer];
        for (int i = 0; i < Pirates.Length; i++)
        {
            Pirates[i] = new Pirate(id, new TilePosition(ShipPosition), PirateType.Usual);
        }
    }
}