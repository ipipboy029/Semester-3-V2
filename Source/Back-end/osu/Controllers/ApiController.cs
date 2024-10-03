using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace osu.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ApiService _apiService;

        public ApiController(ApiService myApiService)
        {
            _apiService = myApiService;
        }

        [HttpGet("{endPoint}")]
        public async Task<IActionResult> GetRequest(string endPoint)
        {
            string result = await _apiService.Request(endPoint);
            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
