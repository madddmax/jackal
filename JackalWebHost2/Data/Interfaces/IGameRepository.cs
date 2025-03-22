using Jackal.Core;

namespace JackalWebHost2.Data.Interfaces;

public interface IGameRepository
{
    Task CreateGame(string gameName, Game game);
}