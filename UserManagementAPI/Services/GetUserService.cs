
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Dtos;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class GetUserService : IGetUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserService(IUserRepository userRepo, IMapper mapper)
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
    }
}