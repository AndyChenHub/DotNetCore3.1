using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using UserManagementAPI.Data.Interfaces;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using Xunit;

namespace UserManagementAPI.Tests.Services
{
    public class UserServiceUnitTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly UserService _service;

        public UserServiceUnitTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
            });
            _mapper = mapperConfig.CreateMapper();

            _mockUserRepository = new Mock<IUserRepository>();
            _service = new UserService(_mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserDto_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var user = new User { UserId = userId, UserName = "Kevin" };
            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(userId))
                               .ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("Kevin", result.UserName);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(userId))
                               .ReturnsAsync((User)null);

            // Act
            var result = await _service.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUsersByFilterAsync_ReturnsUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<User> { new User { UserId = 1, UserName = "Kevin" }, new User { UserId = 2, UserName = "Kevin" } };
            _mockUserRepository.Setup(repo => repo.QueryUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                               .ReturnsAsync(users);

            // Act
            var result = await _service.GetUsersByFilterAsync(username: "Kevin", email: null, alias: null, firstName: null, lastName: null);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.ElementAt(0).UserId);
            Assert.Equal(2, result.ElementAt(1).UserId);
            Assert.Equal("Kevin", result.ElementAt(0).UserName);
            Assert.Equal("Kevin", result.ElementAt(1).UserName);
        }

        [Fact]
        public async Task GetUsersByFilterAsync_ReturnsEmptyList_WhenNoUsersExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.QueryUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                               .ReturnsAsync(new List<User>());

            // Act
            var result = await _service.GetUsersByFilterAsync(username: "NotKevin", email: "NotKevin@example.com", alias: "alias", firstName: "Peter", lastName: "Pan");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CreateUserAsync_CreatesUser()
        {
            // Arrange
            var userDto = new UserDto { UserId = 1, UserName = "Bob" };
            var user = _mapper.Map<User>(userDto);
            _mockUserRepository.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                               .Callback<User>(u => user = u)
                               .Returns(Task.CompletedTask);

            // Act
            await _service.CreateUserAsync(userDto);

            // Assert
            Assert.Equal(userDto.UserId, user.UserId);
            Assert.Equal(userDto.UserName, user.UserName);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            int userId = 1;
            var userDto = new UserDto { UserId = userId, UserName = "Matt" };
            var existingUser = new User { UserId = userId, UserName = "Bob", UserType = UserType.Client };
            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(userId))
                               .ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.QueryClientsByManagerIdAsync(userId))
                               .ReturnsAsync(new List<Client>());

            // Act
            await _service.UpdateUserAsync(userId, userDto);

            // Assert
            Assert.Equal(userId, existingUser.UserId);
            Assert.Equal("Matt", existingUser.UserName);
        }

        [Fact]
        public async Task UpdateUserAsync_ThrowsArgumentException_WhenUserNotFound()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(userId))
                               .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateUserAsync(userId, new UserDto()));
        }

        [Fact]
        public async Task UpdateUserAsync_ThrowsInvalidOperationException_WhenManagerHasClients()
        {
            // Arrange
            int userId = 1;
            var userDto = new UserDto { UserId = userId, UserType = UserType.Client };
            var existingUser = new User { UserId = userId, UserName = "Bob", UserType = UserType.Manager };
            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(userId))
                               .ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.QueryClientsByManagerIdAsync(userId))
                               .ReturnsAsync(new List<Client> { new Client() });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateUserAsync(userId, userDto));
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            // Arrange
            int userId = 1;
            var user = new User { UserId = userId, UserName = "Kevin", UserType = UserType.Client };
            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(userId))
                               .ReturnsAsync(user);

            // Act
            await _service.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteClientAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ThrowsArgumentException_WhenUserNotFound()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(userId))
                               .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteUserAsync(userId));
        }

        [Fact]
        public async Task DeleteUserAsync_ThrowsInvalidOperationException_WhenDeletingManagerWithClients()
        {
            // Arrange
            int userId = 1;
            var user = new User { UserId = userId, UserName = "Kevin", UserType = UserType.Manager };
            _mockUserRepository.Setup(repo => repo.QueryUserByIdAsync(userId))
                               .ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.QueryClientsByManagerIdAsync(userId))
                               .ReturnsAsync(new List<Client> { new Client() });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteUserAsync(userId));
        }
    }
}
