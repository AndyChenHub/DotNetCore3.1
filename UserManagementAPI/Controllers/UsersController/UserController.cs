using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Data;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Models;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Controllers.UsersController
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IGetUserService _service;
        public UsersController(IGetUserService service)
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
    }
}