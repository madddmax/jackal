using JackalWebHost2.Controllers.Models.Map;
using JackalWebHost2.Models.Map;

namespace JackalWebHost2.Services;

public interface IMapService
{
    List<CheckLandingResult> CheckLanding(CheckLandingRequest request);
}