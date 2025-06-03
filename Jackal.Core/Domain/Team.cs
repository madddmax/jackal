using Jackal.Core.Players;
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
    /// ИД пользователя
    /// </summary>
    public readonly long UserId;
    
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
    public Team(int id, string name, long userId, Position shipPosition, Pirate[] pirates)
    {
        Id = id;
        Name = name;
        UserId = userId;
        ShipPosition = shipPosition;
        Pirates = pirates;
    }
    
    public Team(int id, IPlayer player, int x, int y, int piratesPerPlayer)
    {
        Id = id;
        ShipPosition = new Position(x, y);
        
        if (player is IHumanPlayer humanPlayer)
        {
            UserId = humanPlayer.UserId;
            Name = humanPlayer.Name;
        }
        else
        {
            UserId = 0;
            Name = player.GetType().Name;
        }
        
        Pirates = new Pirate[piratesPerPlayer];
        for (int i = 0; i < Pirates.Length; i++)
        {
            Pirates[i] = new Pirate(id, new TilePosition(ShipPosition), PirateType.Usual);
        }
    }
}