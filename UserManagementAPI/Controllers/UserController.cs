using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            try
            {
                var result = await _service.GetUserByIdAsync(userId);

                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound($"No users found with the id {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No users found with the id {userId}.");

                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpGet("query")]
        public async Task<ActionResult<User>> GetMultipleUsers(
            [FromQuery] string username,
            [FromQuery] string email,
            [FromQuery] string alias,
            [FromQuery] string firstName,
            [FromQuery] string lastName)
        {
            try
            {
                var result = await _service.GetUsersByFilterAsync(username, email, alias, firstName, lastName);

                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound("No users found with the provided criteria.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the query by filter.");

                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDto user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User is null.");
                }

                await _service.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");

                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpPatch("{userId}")]
        public async Task<ActionResult<User>> UpdateUser(int userId, [FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest("UserDto is null.");
            }

            try
            {
                await _service.UpdateUserAsync(userId, user);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "User update failed.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                return StatusCode(500, "Failed to update user. Please try again later.");
            }
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                await _service.DeleteUserAsync(userId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "User deletion failed.");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "User deletion failed.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user.");
                return StatusCode(500, "Failed to delete user. Please try again later.");
            }
        }
    }
}
