using Jackal.Core;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GameRepositoryStub : IGameRepository
{
    private long _gameId;
    
    public Task<long> CreateGame(Game game)
    {
        return Task.FromResult(_gameId++);
    }
}