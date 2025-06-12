using Jackal.Core;

namespace JackalWebHost2.Data.Interfaces;

public interface IGameRepository
{
    Task<long> CreateGame(long userId, Game game);

    Task UpdateGame(long gameId, Game game);
}