using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Dtos;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int userId);

        Task<IEnumerable<UserDto>> GetUsersByFilterAsync(string? username, string? email, string? alias, string? firstName, string? lastName);

        Task CreateUserAsync(UserDto userDto);

        Task UpdateUserAsync(int userId, UserDto userDto);

        Task DeleteUserAsync(int userId);
    }
}
