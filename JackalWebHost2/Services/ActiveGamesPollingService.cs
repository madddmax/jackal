using Jackal.Core;
using JackalWebHost2.Controllers.Hubs;
using JackalWebHost2.Controllers.Models.Services;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace JackalWebHost2.Services
{
    public class ActiveGamesPollingService : BackgroundService
    {
        private const string CALLBACK_GET_ACTIVE_GAMES = "GetActiveGames";

        private readonly IServiceProvider _services;
        private readonly IStateRepository<Game> _gameStateRepository;

        public ActiveGamesPollingService(
            IServiceProvider services,
            IStateRepository<Game> gameStateRepository)
        {
            _services = services;
            _gameStateRepository = gameStateRepository;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_gameStateRepository.HasChanges())
                {
                    using var scope = _services.CreateScope();
                    var gameHubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
                    var currentValue = new AllActiveGamesResponse
                    {
                        GamesEntries = _gameStateRepository.GetEntries().Select(ToActiveGame).ToList()
                    };
                    await gameHubContext.Clients.All.SendAsync(CALLBACK_GET_ACTIVE_GAMES, currentValue, stoppingToken);
                    _gameStateRepository.ResetChanges();
                }

                await Task.Delay(15000, stoppingToken);
            }
        }

        private ActiveGameInfo ToActiveGame(CacheEntry entry)
        {
            return new ActiveGameInfo
            {
                GameId = entry.ObjectId,
                Creator = entry.Creator,
                TimeStamp = entry.TimeStamp
            };
        }
    }
}
