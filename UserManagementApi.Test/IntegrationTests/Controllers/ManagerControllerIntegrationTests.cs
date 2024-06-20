using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace UserManagementAPI.Tests.Controllers
{
    public class ManagersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ManagersControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetManagersWithClients_ReturnsOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/managers/clients");

            // Assert
            response.EnsureSuccessStatusCode();
            var managers = JsonSerializer.Deserialize<List<ManagerDto>>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(managers);
        }

        [Fact]
        public async Task GetClientsByManagerUsername_ReturnsOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();
            var username = "John_Smith";

            // Act
            var response = await client.GetAsync($"/api/managers/{username}/clients");

            // Assert
            response.EnsureSuccessStatusCode();
            var clients = JsonSerializer.Deserialize<List<ClientDto>>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(clients);
        }
    }
}
