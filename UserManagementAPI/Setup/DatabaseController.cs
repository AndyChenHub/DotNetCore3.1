using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Data;

namespace UserManagementAPI.Setup
{
    [Route("api")]
    [ApiController]
    public class DataInitializationController : ControllerBase
    {
        private readonly UserDbContext _context;

        public DataInitializationController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost("InitialiseDb")]
        public IActionResult InitialiseDatabase()
        {
            DbInitialiser.Initialise(_context);
            return Ok("Database initialized successfully.");
        }
    }
}
