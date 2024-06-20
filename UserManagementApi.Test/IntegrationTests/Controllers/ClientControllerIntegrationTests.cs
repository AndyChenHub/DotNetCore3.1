using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using UserManagementAPI.Dtos;
using UserManagementAPI.Services.Interfaces;
using Xunit;

namespace UserManagementAPI.Tests.Integration
{
    public class ClientsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ClientsControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllClientsWithManagers_Returns_Ok()
        {
            // Arrange
            var mockClientService = new Mock<IClientService>();
            mockClientService.Setup(service => service.GetAllClientsWithManagersAsync())
                             .ReturnsAsync(new List<ClientWithManagerDto>
                             {
                                 new ClientWithManagerDto { ClientId = 1, UserName = "Matt", ManagerUserName = "Bob" },
                                 new ClientWithManagerDto { ClientId = 2, UserName = "Kevin", ManagerUserName = "Peter" }
                             });

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(mockClientService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/api/clients");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var clientsWithManagers = JsonConvert.DeserializeObject<List<ClientWithManagerDto>>(responseString);

            Assert.Equal(2, clientsWithManagers.Count);
            Assert.Equal("Matt", clientsWithManagers[0].UserName);
            Assert.Equal("Bob", clientsWithManagers[0].ManagerUserName);
            Assert.Equal("Kevin", clientsWithManagers[1].UserName);
            Assert.Equal("Peter", clientsWithManagers[1].ManagerUserName);
        }
    }
}
