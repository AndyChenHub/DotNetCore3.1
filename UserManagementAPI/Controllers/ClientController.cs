// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Threading.Tasks;
// using UserManagementAPI.Services.Interfaces;

// namespace UserManagementAPI.Controllers
// {
//     [Route("api/clients")]
//     [ApiController]
//     public class ClientsController : ControllerBase
//     {
//         private readonly IUserService _userService;

//         public ClientsController(IUserService userService)
//         {
//             _userService = userService;
//         }

//         // GET: api/clients
//         [HttpGet]
//         public async Task<ActionResult> GetAllClientsWithManagers()
//         {
//             try
//             {
//                 var clientsWithManagers = await _userService.GetAllClientsWithManagersAsync();
//                 return Ok(clientsWithManagers);
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception here
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//     }
// }
