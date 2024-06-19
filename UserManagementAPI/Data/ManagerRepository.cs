using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly UserDbContext _context;
        private readonly ILogger<ManagerRepository> _logger;

        public ManagerRepository(UserDbContext context, ILogger<ManagerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Manager>> GetAllManagersWithClientsAsync()
        {
            try
            {
                _logger.LogInformation("Getting all managers with clients");
                return await _context.Managers
                    .Include(m => m.Clients)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all managers with clients");
                throw;
            }
        }

        public async Task<IEnumerable<Client>> GetClientsByManagerUsernameAsync(string username)
        {
            try
            {
                _logger.LogInformation($"Getting clients for manager with username '{username}'");
                return await _context.Clients
                    .Where(c => c.Manager.User.UserName == username)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting clients for manager with username '{username}'");
                throw;
            }
        }
    }
}