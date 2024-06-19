using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Dtos;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientWithManagerDto>> GetAllClientsWithManagersAsync();
    }
}
