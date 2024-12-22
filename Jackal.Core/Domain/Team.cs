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

    public Team(int id, string name, Position shipPosition, Pirate[] pirates)
    {
        Id = id;
        Name = name;
        ShipPosition = shipPosition;
        Pirates = pirates;
    }
}