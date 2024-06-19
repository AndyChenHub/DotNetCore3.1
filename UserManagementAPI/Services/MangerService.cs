using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IUserRepository _userRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IMapper _mapper;

        public ManagerService(IManagerRepository managerRepository, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _managerRepository = managerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ManagerDto>> GetAllManagersWithClientsAsync()
        {
            var managers = await _managerRepository.GetAllManagersWithClientsAsync();
            var managerDtos = _mapper.Map<IEnumerable<ManagerDto>>(managers);

            foreach (var managerDto in managerDtos)
            {
                var user = await _userRepository.QueryUserByIdAsync(managerDto.ManagerId);
                managerDto.UserName = user.UserName;
                foreach (var clientDto in managerDto.Clients)
                {
                    var client = await _userRepository.QueryUserByIdAsync(clientDto.ClientId);
                    clientDto.UserName = client.UserName;
                }
            }

            return managerDtos;
        }


        public async Task<IEnumerable<ClientDto>> GetClientsByManagerUsernameAsync(string username)
        {
            var clients = await _managerRepository.GetClientsByManagerUsernameAsync(username);
            var clientDtos = _mapper.Map<IEnumerable<ClientDto>>(clients);

            foreach (var clientDto in clientDtos)
            {
                var user = await _userRepository.QueryUserByIdAsync(clientDto.ClientId);
                clientDto.UserName = user.UserName;
            }
            return clientDtos;
        }
    }
}
