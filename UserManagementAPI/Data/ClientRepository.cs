using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly UserDbContext _context;

        public ClientRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllClientsWithManagerAsync()
        {
            return await _context.Clients
                .Include(c => c.Manager)
                .ToListAsync();
        }
    }
}
