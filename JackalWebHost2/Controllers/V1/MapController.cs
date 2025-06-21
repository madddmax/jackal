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
    /// Состав клеток игрового набора
    /// </summary>
    [HttpGet("tiles-types")]
    public List<TilesTypeModel> GetTiles(string tilesPackName)
    {
        return TilesPackFactory.Create(tilesPackName).AllTiles
            .GroupBy(p => p.Type)
            .Select(g => new TilesTypeModel
            {
                Name = g.Key.ToString(),
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .ToList();
    }
    
    /// <summary>
    /// Проверить высадку
    /// </summary>
    [HttpGet("check-landing")]
    public List<CheckLandingResponse> CheckLanding([FromQuery] CheckLandingRequest request)
    {
        var landingResults = mapService.CheckLanding(request);
        return landingResults.Select(ToCheckLandingResponse).ToList();
    }

    private static CheckLandingResponse ToCheckLandingResponse(CheckLandingResult landing) =>
        new()
        {
            Position = landing.Position,
            Difficulty = landing.Difficulty
        };
}