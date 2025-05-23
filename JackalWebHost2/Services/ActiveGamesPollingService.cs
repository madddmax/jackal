using JackalWebHost2.Controllers.Hubs;
using JackalWebHost2.Controllers.Models.Services;
using JackalWebHost2.Data.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace JackalWebHost2.Services
{
    public class ActiveGamesPollingService : BackgroundService
    {
        private const string CALLBACK_GET_ACTIVE_GAMES = "GetActiveGames";

        private readonly IServiceProvider _services;
        private readonly IGameStateRepository _gameStateRepository;

        public ActiveGamesPollingService(
            IServiceProvider services, 
            IGameStateRepository gameStateRepository)
        {
            _services = services;
            _gameStateRepository = gameStateRepository;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_gameStateRepository.HasGamesChanges())
                {
                    using var scope = _services.CreateScope();
                    var gameHubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
                    var currentValue = new AllActiveGamesResponse
                    {
                        GamesEntries = _gameStateRepository.GetGamesEntries()
                    };
                    await gameHubContext.Clients.All.SendAsync(CALLBACK_GET_ACTIVE_GAMES, currentValue, stoppingToken);
                    _gameStateRepository.ResetGamesChanges();
                }

                await Task.Delay(15000, stoppingToken);
            }
        }
    }
}
