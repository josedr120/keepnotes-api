using System.Threading.Tasks;
using keepnotes_api.Helpers;
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
        private readonly JwtUtils _jwt;

        public AuthController(AuthService authService, JwtUtils jwtUtils)
        {
            _authService = authService;
            _jwt = jwtUtils;
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

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var response = _jwt.GenerateRefreshToken(refreshToken);

            return Ok(response);
        }
        
        
        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            } 
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}