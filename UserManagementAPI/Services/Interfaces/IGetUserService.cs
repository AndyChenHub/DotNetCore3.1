using System.Collections.Generic;
using UserManagementAPI.Dtos;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IGetUserService
    {
        public UserDto GetUserById(int userId);

        public IEnumerable<UserDto> GetUsersByFilter(string? username, string? email, string? alias, string? firstName, string? lastName);

    }
}