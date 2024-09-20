using System.Collections.Generic;

namespace Jackal.Core
{
    /// <summary>
    /// Задача на поиск доступных ходов
    /// </summary>
    /// <param name="TeamId">ИД команды</param>
    /// <param name="Source">Текущая позиция</param>
    /// <param name="Prev">Предыдущая позиция</param>
    public record AvailableMovesTask(int TeamId, TilePosition Source, TilePosition Prev)
    {
        /// <summary>
        /// Просмотренные позиции
        /// </summary>
        public readonly List<CheckedPosition> AlreadyCheckedList = [];
    }
}