using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IUserRepository userRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientWithManagerDto>> GetAllClientsWithManagersAsync()
        {
            var clients = await _clientRepository.GetAllClientsWithManagerAsync();
            var clientDtos = _mapper.Map<IEnumerable<ClientWithManagerDto>>(clients);

            foreach (var clientDto in clientDtos)
            {
                var user = await _userRepository.QueryUserByIdAsync(clientDto.ClientId);
                clientDto.UserName = user.UserName;
                var managerUser = await _userRepository.QueryUserByIdAsync(clientDto.ManagerId);
                clientDto.ManagerUserName = managerUser.UserName;
            }

            return clientDtos;
        }

    }
}