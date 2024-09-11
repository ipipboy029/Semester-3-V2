using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ApexLegends.Controllers
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
        [HttpGet]
        public async Task<IActionResult> GetPlayer() 
        {
            string result = await _apiService.GetData();
            return Ok(result);
        }
    }
}
