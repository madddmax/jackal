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
    /// Имя игрока (пользователя/бота)
    /// </summary>
    public readonly string PlayerName;

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

    /// <summary>
    /// Бутылки с ромом
    /// </summary>
    public int RumBottles;

    [JsonConstructor]
    public Team(int id, string playerName, long userId, Position shipPosition, Pirate[] pirates)
    {
        Id = id;
        PlayerName = playerName;
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
            PlayerName = humanPlayer.Name;
        }
        else
        {
            UserId = 0;
            PlayerName = player.GetType().Name;
        }
        
        Pirates = new Pirate[piratesPerPlayer];
        for (int i = 0; i < Pirates.Length; i++)
        {
            Pirates[i] = new Pirate(id, new TilePosition(ShipPosition), PirateType.Usual);
        }
    }
}