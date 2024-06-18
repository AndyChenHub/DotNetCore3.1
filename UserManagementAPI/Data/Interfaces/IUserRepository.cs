using System.Collections.Generic;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data.Interfaces
{
    public interface IUserRepository
    {
        public User QueryUserById (int id);

        public IEnumerable<User> QueryUser(string? username, string? email, string? alias, string? firstName, string? lastName);
    }
}