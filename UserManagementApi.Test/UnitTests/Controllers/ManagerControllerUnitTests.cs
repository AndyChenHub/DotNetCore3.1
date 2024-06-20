using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagementAPI.Controllers;
using UserManagementAPI.Services.Interfaces;
using UserManagementAPI.Dtos;
using Xunit;
using UserManagementAPI.Models;

namespace UserManagementAPI.Tests
{
    public class ManagersControllerUnitTests
    {
        private readonly Mock<IManagerService> _mockManagerService;
        private readonly Mock<ILogger<ManagersController>> _mockLogger;
        private readonly ManagersController _controller;

        public ManagersControllerUnitTests()
        {
            _mockManagerService = new Mock<IManagerService>();
            _mockLogger = new Mock<ILogger<ManagersController>>();
            _controller = new ManagersController(_mockManagerService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetManagersWithClients_ReturnsOkResult_WithListOfManagers()
        {
            // Arrange
            var managers = new List<ManagerDto> { new ManagerDto { ManagerId = 1, UserName = "Matt", Clients = new List<ClientDto> { new ClientDto { ClientId = 1, UserName = "David" } } } };
            _mockManagerService.Setup(service => service.GetAllManagersWithClientsAsync())
                .ReturnsAsync(managers);

            // Act
            var result = await _controller.GetManagersWithClients();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ManagerDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetManagersWithClients_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockManagerService.Setup(service => service.GetAllManagersWithClientsAsync())
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetManagersWithClients();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetClientsByManagerUsername_ReturnsOkResult_WithListOfClients()
        {
            // Arrange
            var clients = new List<ClientDto> { new ClientDto { ClientId = 1, UserName = "Matt" } };
            _mockManagerService.Setup(service => service.GetClientsByManagerUsernameAsync("David"))
                .ReturnsAsync(clients);

            // Act
            var result = await _controller.GetClientsByManagerUsername("David");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ClientDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetClientsByManagerUsername_ReturnsNotFoundResult_WhenNoClientsFound()
        {
            // Arrange
            _mockManagerService.Setup(service => service.GetClientsByManagerUsernameAsync("David"))
                .ReturnsAsync(new List<ClientDto>());

            // Act
            var result = await _controller.GetClientsByManagerUsername("David");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No clients found for manager with username 'David'.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetClientsByManagerUsername_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockManagerService.Setup(service => service.GetClientsByManagerUsernameAsync("David"))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetClientsByManagerUsername("David");

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error", statusCodeResult.Value);
        }
    }
}
