using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        // GET: api/clients
        // This api gets all clients with managers
        [HttpGet]
        public async Task<ActionResult> GetAllClientsWithManagers()
        {
            try
            {
                _logger.LogInformation("Getting all clients with managers.");
                var clientsWithManagers = await _clientService.GetAllClientsWithManagersAsync();
                return Ok(clientsWithManagers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to get all clients with managers.");
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }
    }
}