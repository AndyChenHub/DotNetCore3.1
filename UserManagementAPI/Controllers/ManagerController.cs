using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Controllers
{
    [Route("api/managers")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly ILogger<ManagersController> _logger;

        public ManagersController(IManagerService managerService, ILogger<ManagersController> logger)
        {
            _managerService = managerService;
            _logger = logger;
        }

        // GET: api/managers/clients
        // This api gets all managers with clients
        [HttpGet("clients")]
        public async Task<ActionResult> GetManagersWithClients()
        {
            try
            {
                _logger.LogInformation("Getting all managers with clients.");
                var managersWithClients = await _managerService.GetAllManagersWithClientsAsync();
                return Ok(managersWithClients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all managers with clients.");
                return StatusCode(500, ex);
            }
        }

        // GET: api/managers/{username}/clients
        // This api gets all clients by a manager's username
        [HttpGet("{username}/clients")]
        public async Task<ActionResult> GetClientsByManagerUsername(string username)
        {
            try
            {
                _logger.LogInformation($"Getting clients for manager with username '{username}'.");
                var clients = await _managerService.GetClientsByManagerUsernameAsync(username);
                if (clients.Any())
                {
                    return Ok(clients);
                }
                _logger.LogWarning($"No clients found for manager with username '{username}'.");
                return NotFound($"No clients found for manager with username '{username}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting clients for manager with username '{username}'.");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
