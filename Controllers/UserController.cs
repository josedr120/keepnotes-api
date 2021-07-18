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
        public ActionResult<List<User>> GetUsers() => _userService.GetUsers();

        [HttpGet("{id:length(24)}")]
        public ActionResult<User> GetUser(string id)
        {
            var user = _userService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /*[HttpPut("{id:length(24)}")]
        public ActionResult<UserDto> UpdateUser(string id, User updatedUser)
        {
            var user = _userService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }
            
            _userService.UpdateUser(id, updatedUser);

            return NoContent();
        }*/

        [HttpDelete("{id:length(24)}")]
        public ActionResult<User> DeleteUser(string id)
        {
            var user = _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            
            _userService.DeleteUser(id);

            return NoContent();
        }


    }
}