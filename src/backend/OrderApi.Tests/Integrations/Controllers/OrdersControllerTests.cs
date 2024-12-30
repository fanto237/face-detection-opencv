// using System.Text;
// using System.Text.Json;
// using FluentAssertions;
// using Microsoft.VisualStudio.TestPlatform.TestHost;
// using OrderApi.Models;
//
// namespace OrderApi.Tests.Integrations.Controllers;
//
// public class OrdersControllerTests(CustomWebApplicationFactory<Program> factory)
//     : IClassFixture<CustomWebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client = factory.CreateClient();
//
//     [Fact]
//     public async Task PostOrder_ShouldCreateOrder()
//     {
//         // Arrange
//         var newOrder = new OrderCreateDto
//         {
//             Username = "testuser",
//             Email = "testuser@example.com",
//             ImageName = "testimage.jpg",
//             OrderId = Guid.NewGuid()
//         };
//
//         var json = JsonSerializer.Serialize(newOrder);
//         var content = new StringContent(json, Encoding.UTF8, "application/json");
//
//         // Act
//         var response = await _client.PostAsync("/api/v0/orders", content);
//
//         // Assert
//         response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
//
//         var responseContent = await response.Content.ReadAsStringAsync();
//         responseContent.Should().Contain("testuser");
//     }
// }