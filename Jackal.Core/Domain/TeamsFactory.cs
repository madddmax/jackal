using System;

namespace Jackal.Core.Domain;

public static class TeamsFactory
{
    public static Team[] Create(GameRequest request)
    {
        var players = request.Players;
        var teams = new Team[players.Length];
        
        switch (players.Length)
        {
            case 1:
                teams[0] = new Team(0, players[0].GetType().Name, (request.MapSize - 1) / 2, 0, request.PiratesPerPlayer);
                teams[0].EnemyTeamIds = [];
                break;
            case 2:
                teams[0] = new Team(0, players[0].GetType().Name, (request.MapSize - 1) / 2, 0, request.PiratesPerPlayer);
                teams[0].EnemyTeamIds = [1];
                
                teams[1] = new Team(1, players[1].GetType().Name, (request.MapSize - 1) / 2, (request.MapSize - 1), request.PiratesPerPlayer);
                teams[1].EnemyTeamIds = [0];
                break;
            case 4:
                teams[0] = new Team(0, players[0].GetType().Name, (request.MapSize - 1) / 2, 0, request.PiratesPerPlayer);
                teams[1] = new Team(1, players[1].GetType().Name, 0, (request.MapSize - 1) / 2, request.PiratesPerPlayer);
                teams[2] = new Team(2, players[2].GetType().Name, (request.MapSize - 1) / 2, (request.MapSize- 1), request.PiratesPerPlayer);
                teams[3] = new Team(3, players[3].GetType().Name, (request.MapSize - 1), (request.MapSize - 1) / 2, request.PiratesPerPlayer);

                if (request.GameMode == GameModeType.TwoPlayersInTeam)
                {
                    teams[0].EnemyTeamIds = [1, 3];
                    teams[0].AllyTeamId = 2;
                    
                    teams[1].EnemyTeamIds = [0, 2];
                    teams[1].AllyTeamId = 3;
                    
                    teams[2].EnemyTeamIds = [1, 3];
                    teams[2].AllyTeamId = 0;
                    
                    teams[3].EnemyTeamIds = [0, 2];
                    teams[3].AllyTeamId = 1;
                }
                else
                {
                    teams[0].EnemyTeamIds = [1, 2, 3];
                    teams[1].EnemyTeamIds = [0, 2, 3];
                    teams[2].EnemyTeamIds = [0, 1, 3];
                    teams[3].EnemyTeamIds = [0, 1, 2];
                }

                break;
            default:
                throw new NotSupportedException("Only one player, two players or four");
        }

        return teams;
    }
}