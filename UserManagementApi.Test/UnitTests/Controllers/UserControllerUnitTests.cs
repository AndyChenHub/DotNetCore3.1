using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserManagementAPI.Controllers.UsersController;
using UserManagementAPI.Dtos;
using UserManagementAPI.Services.Interfaces;
using Xunit;

namespace UserManagementAPI.Tests.Controllers
{
    public class UsersControllerUnitTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _controller;

        public UsersControllerUnitTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_mockUserService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetUserById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var userDto = new UserDto { UserId = userId, UserName = "Bob" };
            _mockUserService.Setup(repo => repo.GetUserByIdAsync(userId))
                            .ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userId, returnedUser.UserId);
            Assert.Equal("Bob", returnedUser.UserName);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _mockUserService.Setup(repo => repo.GetUserByIdAsync(userId))
                            .ReturnsAsync((UserDto)null);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"No users found with the id {userId}", notFoundResult.Value);
        }

        [Fact]
        public async Task GetMultipleUsers_ReturnsOk_WhenUsersExist()
        {
            // Arrange
            var users = new List<UserDto> { new UserDto { UserId = 1, UserName = "BobBi" }, new UserDto { UserId = 2, UserName = "SamSing" } };
            _mockUserService.Setup(repo => repo.GetUsersByFilterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(users);

            // Act
            var result = await _controller.GetMultipleUsers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
            Assert.Equal("BobBi", returnedUsers[0].UserName);
            Assert.Equal("SamSing", returnedUsers[1].UserName);
        }

        [Fact]
        public async Task GetMultipleUsers_ReturnsNotFound_WhenNoUsersExist()
        {
            // Arrange
            _mockUserService.Setup(repo => repo.GetUsersByFilterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(new List<UserDto>());

            // Act
            var result = await _controller.GetMultipleUsers(username: "KevinSpacey", email: "KevinSpacey@hotmail.com", alias: "KevinSpacey2", firstName: "Kevin", lastName: "Spacey");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No users found with the provided criteria.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreated()
        {
            // Arrange
            var userDto = new UserDto { UserId = 1, UserName = "KevinSpacey" };

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(UsersController.GetUserById), createdAtActionResult.ActionName);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent()
        {
            // Arrange
            int userId = 1;
            var userDto = new UserDto { UserId = userId, UserName = "BobMarley" };

            _mockUserService.Setup(s => s.UpdateUserAsync(userId, userDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result.Result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenUserIsNull()
        {
            // Arrange
            int userId = 1;

            _mockUserService.Setup(s => s.UpdateUserAsync(userId, null)).Throws(new ArgumentNullException());

            // Act
            var result = await _controller.UpdateUser(userId, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("User is null.", badRequestResult.Value);
        }


        [Fact]
        public async Task DeleteUser_ReturnsNoContent()
        {
            // Arrange
            int userId = 1;

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _mockUserService.Setup(repo => repo.DeleteUserAsync(userId))
                            .ThrowsAsync(new ArgumentException($"User with Id {userId} not found."));

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with Id {userId} not found.", notFoundResult.Value);
        }
    }
}
