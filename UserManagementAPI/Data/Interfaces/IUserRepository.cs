using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> QueryUserByIdAsync(int id);
        Task<IEnumerable<User>> QueryUserAsync(string? username, string? email, string? alias, string? firstName, string? lastName);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

        Task<IEnumerable<Client>> QueryClientsByManagerIdAsync(int managerId);
        Task DeleteManagerAsync(int managerId);

        Task DeleteClientAsync(int clientId);
    }
}
