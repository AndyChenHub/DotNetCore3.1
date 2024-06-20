using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly UserDbContext _context;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(UserDbContext context, ILogger<ClientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Client>> GetAllClientsWithManagerAsync()
        {
            try
            {
                _logger.LogInformation("Getting all clients with their managers.");
                var clientsWithManagers = await _context.Clients
                    .Include(c => c.Manager)
                    .ToListAsync();
                return clientsWithManagers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all clients with their managers.");
                throw;
            }
        }
    }
}
