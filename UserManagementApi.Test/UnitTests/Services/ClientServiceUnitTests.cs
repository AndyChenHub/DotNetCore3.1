using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using Xunit;

namespace UserManagementAPI.Tests
{
    public class ClientServiceUnitTests
    {
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly ClientService _service;

        public ClientServiceUnitTests()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Client, ClientWithManagerDto>();
            });
            _mapper = config.CreateMapper();

            _service = new ClientService(_mockClientRepository.Object, _mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllClientsWithManagersAsync_ReturnsClientsWithManagerDtos()
        {
            // Arrange
            var clients = new List<Client> { new Client { ClientId = 1, Manager = new Manager(), ManagerId = 2 } };
            var users = new List<User>
            {
                new User { UserId = 1, UserName = "Kevin" },
                new User { UserId = 2, UserName = "Matt" }
            };

            _mockClientRepository.Setup(repo => repo.GetAllClientsWithManagerAsync())
                .ReturnsAsync(clients);

            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => users.FirstOrDefault(u => u.UserId == id));

            // Act
            var result = await _service.GetAllClientsWithManagersAsync();

            // Assert
            var clientDtos = Assert.IsType<List<ClientWithManagerDto>>(result);
            Assert.Single(clientDtos);
            Assert.Equal("Kevin", clientDtos.First().UserName);
            Assert.Equal("Matt", clientDtos.First().ManagerUserName);
        }
    }
}
