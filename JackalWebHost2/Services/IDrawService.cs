using Jackal.Core;
using JackalWebHost2.Models;

namespace JackalWebHost2.Services;

public interface IDrawService
{
    /// <summary>
    /// Получить изменения пиратов
    /// </summary>
    List<PirateChange> GetPirateChanges(Board board, Board prevBoard);

    /// <summary>
    /// Получить изменения клеток
    /// </summary>
    public List<TileChange> GetTileChanges(Board board, Board prevBoard);

    /// <summary>
    /// Формирование статистики после хода
    /// </summary>
    GameStatistics GetStatistics(Game game);

    /// <summary>
    /// Получить доступные ходы
    /// </summary>
    List<DrawMove> GetAvailableMoves(Game game);

    /// <summary>
    /// Отрисовать карту
    /// </summary>
    DrawMap Map(Board board);
}