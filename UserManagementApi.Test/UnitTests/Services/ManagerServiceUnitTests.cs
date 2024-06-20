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
    public class ManagerServiceUnitTests
    {
        private readonly Mock<IManagerRepository> _mockManagerRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly ManagerService _service;

        public ManagerServiceUnitTests()
        {
            _mockManagerRepository = new Mock<IManagerRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Manager, ManagerDto>()
                    .ForMember(dest => dest.Clients, opt => opt.MapFrom(src => src.Clients));
                cfg.CreateMap<Client, ClientDto>();
            });
            _mapper = config.CreateMapper();

            _service = new ManagerService(_mockManagerRepository.Object, _mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllManagersWithClientsAsync_ReturnsManagersWithClients()
        {
            // Arrange
            var managers = new List<Manager>
            {
                new Manager
                {
                    ManagerId = 1,
                    Clients = new List<Client>
                    {
                        new Client
                        {
                            ClientId = 1,
                            User = new User { UserId = 1, UserName = "Bruce" }
                        }
                    }
                }
            };

            var users = new List<User>
            {
                new User { UserId = 1, UserName = "Bruce" },
                new User { UserId = 2, UserName = "John" }
            };

            _mockManagerRepository.Setup(repo => repo.GetAllManagersWithClientsAsync())
                .ReturnsAsync(managers);

            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => users.FirstOrDefault(u => u.UserId == id));

            // Act
            var result = await _service.GetAllManagersWithClientsAsync();

            // Assert
            var managerDtos = Assert.IsType<List<ManagerDto>>(result);
            Assert.Single(managerDtos);
            Assert.Equal("Bruce", managerDtos.First().UserName);
            Assert.Equal("Bruce", managerDtos.First().Clients.First().UserName);
        }


        [Fact]
        public async Task GetClientsByManagerUsernameAsync_ReturnsClientsByManagerUsername()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { ClientId = 1 }
            };

            var user = new User { UserId = 2, UserName = "Bruce" };

            _mockManagerRepository.Setup(repo => repo.GetClientsByManagerUsernameAsync("Kevin"))
                .ReturnsAsync(clients);

            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            // Act
            var result = await _service.GetClientsByManagerUsernameAsync("Kevin");

            // Assert
            var clientDtos = Assert.IsType<List<ClientDto>>(result);
            Assert.Single(clientDtos);
            Assert.Equal("Bruce", clientDtos.First().UserName);
        }
    }
}
