using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> QueryUserAsync(string? username, string? email, string? alias, string? firstName, string? lastName)
        {
            try
            {
                _logger.LogInformation("Querying users with provided filters.");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while querying users with provided filters.");
                throw;
            }
        }

        public async Task<User> QueryUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Querying user with id '{id}'.");
                return await _context.Users.FirstOrDefaultAsync(p => p.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while querying user with id '{id}'.");
                throw;
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogError("User is null.");
                    throw new ArgumentNullException(nameof(user));
                }

                _logger.LogInformation("Adding a new user.");
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user.");
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogError("User is null.");
                    throw new ArgumentNullException(nameof(user));
                }

                _logger.LogInformation($"Updating user with id '{user.UserId}'.");
                _context.Users.Attach(user);
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with id '{user.UserId}'.");
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with id '{id}'.");
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with id '{id}'.");
                throw;
            }
        }

        public async Task<IEnumerable<Client>> QueryClientsByManagerIdAsync(int managerId)
        {
            try
            {
                _logger.LogInformation($"Querying clients by manager id '{managerId}'.");
                return await _context.Clients.Where(c => c.ManagerId == managerId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while querying clients by manager id '{managerId}'.");
                throw;
            }
        }

        public async Task DeleteManagerAsync(int managerId)
        {
            try
            {
                _logger.LogInformation($"Deleting manager with id '{managerId}'.");
                var manager = await _context.Managers.FindAsync(managerId);
                if (manager != null)
                {
                    _context.Managers.Remove(manager);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting manager with id '{managerId}'.");
                throw;
            }
        }

        public async Task DeleteClientAsync(int clientId)
        {
            try
            {
                _logger.LogInformation($"Deleting client with id '{clientId}'.");
                var client = await _context.Clients.FindAsync(clientId);
                if (client != null)
                {
                    _context.Clients.Remove(client);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting client with id '{clientId}'.");
                throw;
            }
        }
    }
}
