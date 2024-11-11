using Jackal.Core;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Jackal.Core.MapGenerator.TilesPack;
using JackalWebHost2.Controllers.Models.Map;
using JackalWebHost2.Models.Map;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers.V1;

[AllowAnonymous]
[Route("/api/v1/map")]
public class MapController(IMapService mapService) : Controller
{
    private readonly IMapService _mapService = mapService;

    /// <summary>
    /// Список названий игровых наборов,
    /// используется при создании игры
    /// </summary>
    [HttpGet("tiles-pack-names")]
    public List<string> TilesPackNames()
    {
        return TilesPackFactory.GetAll();
    }
    
    /// <summary>
    /// Проверить высадку
    /// </summary>
    [HttpGet("check-landing")]
    public List<CheckLandingResult> CheckLanding([FromQuery] CheckLandingRequest request)
    {
        var mapGenerator = new RandomMapGenerator(request.MapId, request.MapSize, request.TilesPackName);
        
        var upLanding = new CheckLandingResult(DirectionType.Up);
        var rightLanding = new CheckLandingResult(DirectionType.Right);
        var downLanding = new CheckLandingResult(DirectionType.Down);
        var leftLanding = new CheckLandingResult(DirectionType.Left);
        
        for (int i = 2; i <= request.MapSize - 3; i++)
        {
            var upTile = mapGenerator.GetNext(new Position(i, request.MapSize - 2));
            SetLandingResult(upLanding, upTile);
            
            var rightTile = mapGenerator.GetNext(new Position(request.MapSize - 2, i));
            SetLandingResult(rightLanding, rightTile);
            
            var downTile = mapGenerator.GetNext(new Position(i, 1));
            SetLandingResult(downLanding, downTile);
            
            var leftTile = mapGenerator.GetNext(new Position(1, i));
            SetLandingResult(leftLanding, leftTile);
        }
        
        return
        [
            upLanding,
            rightLanding,
            downLanding,
            leftLanding
        ];
    }

    private static void SetLandingResult(CheckLandingResult landing, Tile tile)
    {
        landing.Coins += tile.Type.CoinsCount();
        landing.Cannibals += tile.Type == TileType.Cannibal ? 1 : 0;
    }
}