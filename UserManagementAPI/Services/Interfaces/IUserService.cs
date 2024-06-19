using System.Collections.Generic;
using UserManagementAPI.Dtos;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        public UserDto GetUserById(int userId);

        public IEnumerable<UserDto> GetUsersByFilter(string? username, string? email, string? alias, string? firstName, string? lastName);
        void CreateUser(UserDto userDto);
        void UpdateUser(int userId, UserDto userDto);
        void DeleteUser(int userId);
    }
}