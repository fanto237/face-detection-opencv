// using System.Text;
// using System.Text.Json;
// using FluentAssertions;
// using Microsoft.VisualStudio.TestPlatform.TestHost;
// using OrderApi.Models;
//
// namespace OrderApi.Tests.Integrations.Controllers;
//
// public class UsersControllerTests(CustomWebApplicationFactory<Program> factory)
//     : IClassFixture<CustomWebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client = factory.CreateClient();
//
//     [Fact]
//     public async Task Register_ShouldCreateUser()
//     {
//         // Arrange
//         var newUser = new UserRegisterDto("Test User", "testuser", "password123");
//         var json = JsonSerializer.Serialize(newUser);
//         var content = new StringContent(json, Encoding.UTF8, "application/json");
//
//         // Act
//         var response = await _client.PostAsync("/api/v0/usersAuth/register", content);
//
//         // Assert
//         response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//
//         var responseContent = await response.Content.ReadAsStringAsync();
//         responseContent.Should().Contain("testuser");
//         Assert.Contains(responseContent, "testuser");
//     }
// }