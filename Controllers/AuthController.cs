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
        public  ActionResult<AuthenticatedResponse> Register([FromBody] User user)
        {
            var response = _authService.Register(user);

            return Ok(response);
        }
        
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public ActionResult<AuthenticatedResponse> Login([FromBody] Login login)
        {
            var response = _authService.Login(login);

            if (response == null)
            {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}