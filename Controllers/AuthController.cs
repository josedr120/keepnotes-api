using System.Threading.Tasks;
using keepnotes_api.Models;
using keepnotes_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace keepnotes_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        
        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var response = await _authService.Register(user);

            return Ok(response);
        }
        
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<Login>> Login([FromBody] Login login)
        {
            var response = await _authService.Login(login);

            if (response == null)
            {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}