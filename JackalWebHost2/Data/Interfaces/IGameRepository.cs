using Jackal.Core;

namespace JackalWebHost2.Data.Interfaces;

public interface IGameRepository
{
    Task<long> CreateGame(Game game);
}