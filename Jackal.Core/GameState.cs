using Jackal.Core.Domain;

namespace Jackal.Core;

/// <summary>
/// Состояние игры
/// </summary>
public class GameState
{
    /// <summary>
    /// Данные игры: карта + информация о командах
    /// </summary>
    public Board Board;
    
    /// <summary>
    /// Доступные ходы
    /// </summary>
    public Move[] AvailableMoves;
    
    /// <summary>
    /// ИД команды пиратов чей ход
    /// </summary>
    public int TeamId;
    
    /// <summary>
    /// ИД игрока чей ход,
    /// отличается от ИД команды при розыгрыше хи-хи травы
    /// </summary>
    public int PlayerId;
}