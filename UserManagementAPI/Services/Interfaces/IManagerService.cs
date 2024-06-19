using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IManagerService
    {
        Task<IEnumerable<ManagerDto>> GetAllManagersWithClientsAsync();
        Task<IEnumerable<ClientDto>> GetClientsByManagerUsernameAsync(string username);
    }
}
