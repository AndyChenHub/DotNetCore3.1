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

        // Constructor
        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/users/{userId}
        // This api gets users with user id
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            try
            {
                _logger.LogInformation($"Getting user with id '{userId}'.");
                var result = await _service.GetUserByIdAsync(userId);

                if (result != null)
                {
                    return Ok(result);
                }
                _logger.LogWarning($"No users found with the id '{userId}'.");
                return NotFound($"No users found with the id {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user with id '{userId}'.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/users/query
        // This api gets user with any combination of username, email, alias, firstname, lastname
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
                _logger.LogInformation("Getting users by filter.");
                var result = await _service.GetUsersByFilterAsync(username, email, alias, firstName, lastName);

                if (result.Any())
                {
                    return Ok(result);
                }
                _logger.LogWarning("No users found with the provided criteria.");
                return NotFound("No users found with the provided criteria.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the query by filter.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: api/users
        // This api creates a new user
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDto user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User is null.");
                }

                _logger.LogInformation("Creating a new user.");
                await _service.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PATCH: api/users/{userId}
        // This api updates users information
        [HttpPatch("{userId}")]
        public async Task<ActionResult<User>> UpdateUser(int userId, [FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest("UserDto is null.");
            }

            try
            {
                _logger.LogInformation($"Updating user with id '{userId}'.");
                await _service.UpdateUserAsync(userId, user);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Failed to update user with id '{userId}'. Invalid operation.");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Failed to update user with id '{userId}'. Argument exception.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with id '{userId}'.");
                return StatusCode(500, "Failed to update user.");
            }
        }

        // DELETE: api/users/{userId}
        // This api delets the a user. If the user is a manager, they must not have any clients before they are deleted.
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                _logger.LogInformation($"Deleting user with id '{userId}'.");
                await _service.DeleteUserAsync(userId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Failed to delete user with id '{userId}'. Argument exception.");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Failed to delete user with id '{userId}'. Invalid operation.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with id '{userId}'.");
                return StatusCode(500, "Failed to delete user.");
            }
        }
    }
}
