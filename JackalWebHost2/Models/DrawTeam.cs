using Jackal.Core.Domain;

namespace JackalWebHost2.Models;

public class DrawTeam(Team team)
{
    /// <summary>
    /// ИД команды
    /// </summary>
    public readonly int Id = team.Id;
    
    /// <summary>
    /// Имя команды
    /// </summary>
    public readonly string Name = team.Name;
    
    /// <summary>
    /// ИД пользователя
    /// </summary>
    public long UserId => team.UserId;

    /// <summary>
    /// Человек
    /// </summary>
    public bool IsHuman => team.UserId > 0;
    
    /// <summary>
    /// Позиция корабля
    /// </summary>
    public DrawPosition Ship = new(team.ShipPosition);
}