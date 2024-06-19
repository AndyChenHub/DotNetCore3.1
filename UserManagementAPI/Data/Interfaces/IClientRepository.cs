using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllClientsWithManagerAsync();
    }
}
