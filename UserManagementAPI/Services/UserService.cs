using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepository = userRepo;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var response = await _userRepository.QueryUserByIdAsync(userId);

            if (response != null)
            {
                var mappedResponse = _mapper.Map<UserDto>(response);
                return mappedResponse;
            }
            return null;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByFilterAsync(string? username, string? email, string? alias, string? firstName, string? lastName)
        {
            var response = await _userRepository.QueryUserAsync(username, email, alias, firstName, lastName);

            if (response != null && response.Any())
            {
                var mappedResponse = _mapper.Map<IEnumerable<UserDto>>(response);
                return mappedResponse;
            }
            return Enumerable.Empty<UserDto>();
        }

        public async Task CreateUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userRepository.AddUserAsync(user);
            var createdUserDto = _mapper.Map<UserDto>(user);
            userDto.UserId = createdUserDto.UserId;
        }

        public async Task UpdateUserAsync(int userId, UserDto userDto)
        {
            var existingUser = await _userRepository.QueryUserByIdAsync(userId);

            if (existingUser == null)
            {
                throw new ArgumentException($"User with Id {userId} not found.");
            }

            if (userDto.UserName != null)
            {
                existingUser.UserName = userDto.UserName;
            }
            if (userDto.Email != null)
            {
                existingUser.Email = userDto.Email;
            }
            if (userDto.Alias != null)
            {
                existingUser.Alias = userDto.Alias;
            }
            if (userDto.FirstName != null)
            {
                existingUser.FirstName = userDto.FirstName;
            }
            if (userDto.LastName != null)
            {
                existingUser.LastName = userDto.LastName;
            }

            if (userDto.UserType == UserType.Client && existingUser.UserType == UserType.Manager)
            {
                var clients = await _userRepository.QueryClientsByManagerIdAsync(userId);
                if (clients.Any())
                {
                    throw new InvalidOperationException("Cannot change UserType to Client because the manager has associated clients.");
                }
            }

            existingUser.UserType = userDto.UserType;

            await _userRepository.UpdateUserAsync(existingUser);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.QueryUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with Id {userId} not found.");
            }

            switch (user.UserType)
            {
                case UserType.Manager:
                    await DeleteManagerAsync(userId);
                    break;
                case UserType.Client:
                    await DeleteClientAsync(userId);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported UserType '{user.UserType}'.");
            }
            await _userRepository.DeleteUserAsync(userId);
        }

        private async Task DeleteManagerAsync(int managerId)
        {
            var manager = await _userRepository.QueryUserByIdAsync(managerId);

            if (manager == null)
            {
                throw new ArgumentException($"Manager with id {managerId} not found.");
            }

            var clients = await _userRepository.QueryClientsByManagerIdAsync(managerId);
            if (clients.Any())
            {
                throw new InvalidOperationException($"Manager with id {managerId} cannot be deleted because it has associated clients.");
            }

            await _userRepository.DeleteManagerAsync(managerId);
        }

        private async Task DeleteClientAsync(int clientId)
        {
            var client = await _userRepository.QueryUserByIdAsync(clientId);

            if (client == null)
            {
                throw new ArgumentException($"Client with id {clientId} not found.");
            }

            await _userRepository.DeleteClientAsync(clientId);
        }
    }
}
