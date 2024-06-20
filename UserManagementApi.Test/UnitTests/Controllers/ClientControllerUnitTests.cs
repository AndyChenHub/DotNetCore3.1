using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagementAPI.Controllers;
using UserManagementAPI.Services.Interfaces;
using Xunit;
using UserManagementAPI.Dtos;

namespace UserManagementAPI.Tests
{
    public class ClientsControllerUnitTests
    {
        private readonly Mock<IClientService> _mockClientService;
        private readonly Mock<ILogger<ClientsController>> _mockLogger;
        private readonly ClientsController _controller;

        public ClientsControllerUnitTests()
        {
            _mockClientService = new Mock<IClientService>();
            _mockLogger = new Mock<ILogger<ClientsController>>();
            _controller = new ClientsController(_mockClientService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllClientsWithManagers_ReturnsOkResult_WithListOfClients()
        {
            // Arrange
            var clients = new List<ClientWithManagerDto> { new ClientWithManagerDto { ClientId = 1, UserName = "David", ManagerUserName = "Matt" } };
            _mockClientService.Setup(service => service.GetAllClientsWithManagersAsync())
                .ReturnsAsync(clients);

            // Act
            var result = await _controller.GetAllClientsWithManagers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ClientWithManagerDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAllClientsWithManagers_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockClientService.Setup(service => service.GetAllClientsWithManagersAsync())
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAllClientsWithManagers();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
