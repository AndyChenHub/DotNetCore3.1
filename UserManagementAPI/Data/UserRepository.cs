using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<User> QueryUser(string? username, string? email, string? alias, string? firstName, string? lastName)
        {
            var query = _context.Users.AsQueryable();

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

            return query.ToList();
        }
        public User QueryUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.UserId == id);
        }
    }
}