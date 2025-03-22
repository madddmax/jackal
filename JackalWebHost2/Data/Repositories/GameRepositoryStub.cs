using Jackal.Core;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GameRepositoryStub : IGameRepository
{
    public Task CreateGame(string gameName, Game game)
    {
        return Task.FromResult(new object());
    }
}