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
        // todo mapSize validator
        //IMapGenerator mapGenerator = new RandomMapGenerator(request.MapId, request.MapSize, request.TilesPackName);
        
        return
        [
            new(0, 0), // нижняя высадка, далее по часовой стрелке
            new(1, 1),
            new(2, 2),
            new(3, 3)
        ];
    }
}