
using System;
using System.Collections.Generic;
using System.Linq;
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

        public UserDto GetUserById(int userId)
        {
            var response = _userRepository.QueryUserById(userId);

            if (response != null)
            {
                var mappedResponse = _mapper.Map<UserDto>(response);
                return mappedResponse;
            }
            return null;
        }

        public IEnumerable<UserDto> GetUsersByFilter(string? username, string? email, string? alias, string? firstName, string? lastName)
        {
            var response = _userRepository.QueryUser(username, email, alias, firstName, lastName);

            if (response != null && response.Any())
            {
                var mappedResponse = _mapper.Map<IEnumerable<UserDto>>(response);
                return mappedResponse;
            }
            return Enumerable.Empty<UserDto>();
        }
        public void CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _userRepository.AddUser(user);
            var createdUserDto = _mapper.Map<UserDto>(user);
            userDto.UserId = createdUserDto.UserId;
        }

public void UpdateUser(int userId, UserDto userDto)
{
    var existingUser = _userRepository.QueryUserById(userId);

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
    if (userDto.UserType != existingUser.UserType)
    {
        existingUser.UserType = userDto.UserType;
    }

    _userRepository.UpdateUser(existingUser);
}

        public void DeleteUser(int userId)
        {
            _userRepository.DeleteUser(userId);
        }
    }
}