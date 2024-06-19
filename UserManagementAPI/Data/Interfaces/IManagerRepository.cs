using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data.Interfaces
{
    public interface IManagerRepository
    {
        Task<IEnumerable<Manager>> GetAllManagersWithClientsAsync();
        Task<IEnumerable<Client>> GetClientsByManagerUsernameAsync(string username);
    }
}
