// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using UserManagementAPI.Services.Interfaces;

// namespace UserManagementAPI.Controllers
// {
//     [Route("api/managers")]
//     [ApiController]
//     public class ManagersController : ControllerBase
//     {
//         private readonly IUserService _userService;

//         public ManagersController(IUserService userService)
//         {
//             _userService = userService;
//         }

//         // GET: api/managers/clients
//         [HttpGet("clients")]
//         public async Task<ActionResult> GetManagersWithClients()
//         {
//             try
//             {
//                 var managersWithClients = await _userService.GetManagersWithClientsAsync();
//                 return Ok(managersWithClients);
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception here
//                 return StatusCode(500, "Internal server error");
//             }
//         }

//         // GET: api/managers/{username}/clients
//         [HttpGet("{username}/clients")]
//         public async Task<ActionResult> GetClientsByManagerUsername(string username)
//         {
//             try
//             {
//                 var clients = await _userService.GetClientsByManagerUsernameAsync(username);
//                 if (clients.Any())
//                 {
//                     return Ok(clients);
//                 }
//                 return NotFound($"No clients found for manager with username '{username}'.");
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception here
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//     }
// }
