using Microsoft.AspNetCore.Mvc;
using MySeries.Series;
using System.Threading.Tasks;

namespace MySeries.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesApiService _seriesApiService;

        public SeriesController(ISeriesApiService seriesApiService)
        {
            _seriesApiService = seriesApiService;
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> BuscarSeriesAsync(string Title, string Genre)
        {
            var result = await _seriesApiService.BuscarSerieAsync(Title, Genre);
            return Ok(result);
        }
    }
}
