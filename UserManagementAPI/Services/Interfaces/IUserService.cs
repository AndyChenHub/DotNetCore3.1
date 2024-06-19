using System.Collections.Generic;
using UserManagementAPI.Dtos;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        public UserDto GetUserById(int userId);

        public IEnumerable<UserDto> GetUsersByFilter(string? username, string? email, string? alias, string? firstName, string? lastName);
        public void CreateUser(UserDto userDto);
        public void UpdateUser(int userId, UserDto userDto);
        public void DeleteUser(int userId);
    }
}