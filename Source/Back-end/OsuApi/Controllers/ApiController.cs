using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace OsuApi.Controllers
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
    }
}
