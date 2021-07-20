using System.Collections.Generic;
using System.Threading.Tasks;
using keepnotes_api.DTOs;
using keepnotes_api.Models;
using keepnotes_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace keepnotes_api.Controllers
{
    /*[Authorize]*/
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.Get();

            return Ok(users);
        }

        [HttpGet("{userId:length(24)}")]
        public async Task<IActionResult> Get(string userId)
        {
            var user = await _userService.Get(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("{userId:length(24)}")]
        public async Task<IActionResult> Update(string userId, User update)
        {
            var user = await _userService.Get(userId);

            if (user == null)
            {
                return NotFound();
            }
            
            await _userService.Update(userId, update);

            return NoContent();
        }

        [HttpDelete("{userId:length(24)}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userService.Get(userId);
            if (user == null)
            {
                return NotFound();
            }
            
            await _userService.Delete(userId);

            return NoContent();
        }


    }
}