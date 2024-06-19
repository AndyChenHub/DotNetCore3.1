using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Data;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Controllers.UsersController
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]
        public ActionResult<User> GetUserById(int userId)
        {
            var result = _service.GetUserById(userId);
            
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"No users found with the id {userId}");
        }

        [HttpGet("query")]
        public ActionResult<User> GetMultipleUsers(
            [FromQuery] string username,
            [FromQuery] string email,
            [FromQuery] string alias,
            [FromQuery] string firstName,
            [FromQuery] string lastName)
        {
            var result = _service.GetUsersByFilter(username, email, alias, firstName, lastName);
            
            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound("No users found with the provided criteria.");
        }


        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest("User is null.");
            }

            _service.CreateUser(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
        }

        [HttpPatch("{userId}")]
        public ActionResult<User> UpdateUser(int userId, [FromBody] UserDto user)
       {
    if (user == null)
    {
        return BadRequest("UserDto is null.");
    }

    var existingUser = _service.GetUserById(userId);
    if (existingUser == null)
    {
        return NotFound($"No user found with id {userId}");
    }

    _service.UpdateUser(userId, user);
    return NoContent();
}

        [HttpDelete("{userId}")]
        public ActionResult DeleteUser(int userId)
        {
            var user = _service.GetUserById(userId);
            if (user == null)
            {
                return NotFound($"No user found with id {userId}");
            }

            _service.DeleteUser(userId);
            return NoContent();
        }
    }
}