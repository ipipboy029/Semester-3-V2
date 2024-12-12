using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static BusinessLayer.Models.UserDTO;

namespace osu.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                await _userService.RegisterUser(registerDto.Email, registerDto.Password);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var response = await _userService.LoginUser(loginDto.Email, loginDto.Password);
                return Ok(response); // Return the user and token in the response
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message); // Handle error and return unauthorized if login fails
            }
        }
    }
}
