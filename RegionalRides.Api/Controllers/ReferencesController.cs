using Constants.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegionalRides.DAL;
using RegionalRides.Services.Interfaces;

namespace RegionalRides.Api.Controllers
{
    [Route("api/regional/[controller]")]
    [ApiController]
    public class ReferencesController : ControllerBase
    {
        private readonly IReferencesService _referencesService;

        public ReferencesController(IReferencesService referencesService)
        {
            _referencesService = referencesService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SyncKatos()
        {
            var res = await _referencesService.KatoSync();
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetKatos(
            [FromQuery] string searchQuery,
            [FromServices] RegionalRidesContext regionalRidesContext,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (searchQuery == null)
                searchQuery = string.Empty;
            var locationTypes = new[] { BnsLocationType.City, BnsLocationType.Village };
            var katos = await regionalRidesContext.RefKatos
                .Where(x => locationTypes.Contains(x.BnsLocationType)
                            && x.NameRu.ToLower().Contains(searchQuery))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new
                {
                    x.Id,
                    Code = x.Te,
                    Name = x.NameRu,
                    Region = x.Region.NameRu ?? string.Empty
                }).ToArrayAsync();
            return Ok(katos);
        }
    }
}