using Jackal.Core;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Jackal.Core.MapGenerator.TilesPack;
using JackalWebHost2.Controllers.Models.Map;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers.V1;

[AllowAnonymous]
[Route("/api/v1/map")]
public class MapController : Controller
{
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
    public List<CheckLandingResponse> CheckLanding([FromQuery] CheckLandingRequest request)
    {
        var mapGenerator = new RandomMapGenerator(request.MapId, request.MapSize, request.TilesPackName);
        
        var upLanding = new CheckLandingResponse(DirectionType.Up);
        var rightLanding = new CheckLandingResponse(DirectionType.Right);
        var downLanding = new CheckLandingResponse(DirectionType.Down);
        var leftLanding = new CheckLandingResponse(DirectionType.Left);
        
        for (int i = 2; i <= request.MapSize - 3; i++)
        {
            var upTile = mapGenerator.GetNext(new Position(i, request.MapSize - 2));
            upLanding.Wealth += upTile.Type.CoinsCount();
            
            var rightTile = mapGenerator.GetNext(new Position(request.MapSize - 2, i));
            rightLanding.Wealth += rightTile.Type.CoinsCount();
            
            var downTile = mapGenerator.GetNext(new Position(i, 1));
            downLanding.Wealth += downTile.Type.CoinsCount();
            
            var leftTile = mapGenerator.GetNext(new Position(1, i));
            leftLanding.Wealth += leftTile.Type.CoinsCount();
        }
        
        return
        [
            upLanding,
            rightLanding,
            downLanding,
            leftLanding
        ];
    }
}