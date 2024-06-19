using System.Collections.Generic;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data.Interfaces
{
    public interface IUserRepository
    {
        public User QueryUserById(int id);
        public IEnumerable<User> QueryUser(string? username, string? email, string? alias, string? firstName, string? lastName);
        public void AddUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(int id);

        public IEnumerable<Client> QueryClientsByManagerId(int managerId);
        public void DeleteManager(int managerId);

        public void DeleteClient(int clientId);
    }
}