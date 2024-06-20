using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using UserManagementAPI.Dtos;
using UserManagementAPI.Test.Helpers;
using Xunit;

namespace UserManagementAPI.Tests.IntegrationTests
{
    public class UsersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public UsersControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _options = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new UserTypeJsonConverter() }
            };
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetUserById_ReturnsUser(int userId)
        {
            var response = await _client.GetAsync($"/api/users/{userId}");
            response.EnsureSuccessStatusCode();
            var user = JsonSerializer.Deserialize<UserDto>(await response.Content.ReadAsStringAsync(), _options);
            Assert.NotNull(user);
            Assert.Equal(userId, user.UserId);
        }

        [Fact]
        public async Task GetMultipleUsers_ReturnsUsers()
        {
            var response = await _client.GetAsync("/api/users/query?username=James&alias=James");
            response.EnsureSuccessStatusCode();
            var users = JsonSerializer.Deserialize<List<UserDto>>(await response.Content.ReadAsStringAsync(), _options);
            Assert.NotNull(users);
            Assert.True(users.Count > 0);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedAtAction()
        {
            var newUser = new UserDto { UserName = "Spiderman", Email = "PeterParker@hotmail.com" };
            var content = new StringContent(JsonSerializer.Serialize(newUser, _options), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/users", content);
            response.EnsureSuccessStatusCode();
            var createdUser = JsonSerializer.Deserialize<UserDto>(await response.Content.ReadAsStringAsync(), _options);
            Assert.NotNull(createdUser);
            Assert.NotEqual(0, createdUser.UserId);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task UpdateUser_ReturnsNoContent(int userId)
        {
            var updateUser = new UserDto { UserName = "BobMarley", Email = "BobMarley@hotmail.com" };
            var content = new StringContent(JsonSerializer.Serialize(updateUser, _options), Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"/api/users/{userId}", content);
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        public async Task DeleteUser_ReturnsNoContent(int userId)
        {
            var response = await _client.DeleteAsync($"/api/users/{userId}");
            response.EnsureSuccessStatusCode();
        }
    }
}