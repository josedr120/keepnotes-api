using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using keepnotes_api.DTOs;
using keepnotes_api.Helpers;
using keepnotes_api.Models;
using keepnotes_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace keepnotes_api.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtUtils _jwt;
        

        public UserController(UserService userService, JwtUtils jwtUtils)
        {
            _userService = userService;
            _jwt = jwtUtils;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<UserDto>> Get()
        {
            var users = await _userService.Get();

            if (users == null)
            {
                return NotFound();
            }
            
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1].Trim();

            var ifTokenIsVerified = _jwt.VerifyJwtToken(token);


            if (ifTokenIsVerified)
            {
                return Ok(users);
            }
            else
            {
                var verifyException = new VerificationException("Token is not verified").Message;
                return Ok(verifyException.ToJson());
            }

        }

        [HttpGet("{userId:length(24)}")]
        public async Task<ActionResult<UserDto>> Get(string userId)
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