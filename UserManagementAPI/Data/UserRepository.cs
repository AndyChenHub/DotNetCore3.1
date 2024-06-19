using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> QueryUserAsync(string? username, string? email, string? alias, string? firstName, string? lastName)
        {
            IQueryable<User> query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(u => u.UserName.Contains(username));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(alias))
            {
                query = query.Where(u => u.Alias.Contains(alias));
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(u => u.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(u => u.LastName.Contains(lastName));
            }

            return await query.ToListAsync();
        }

        public async Task<User> QueryUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Client>> QueryClientsByManagerIdAsync(int managerId)
        {
            return await _context.Clients.Where(c => c.ManagerId == managerId).ToListAsync();
        }

        public async Task DeleteManagerAsync(int managerId)
        {
            var manager = await _context.Managers.FindAsync(managerId);
            if (manager != null)
            {
                _context.Managers.Remove(manager);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteClientAsync(int clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }
    }
}
