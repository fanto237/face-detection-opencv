using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using OrderApi.Models;

namespace OrderApi.Tests.Models;

public class ModelsTests
{
    [Fact]
    public void Face_ShouldHaveCorrectProperties()
    {
        // Arrange
        var faceId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var faceData = new byte[] { 1, 2, 3 };

        // Act
        var face = new Face
        {
            Id = faceId,
            OrderId = orderId,
            FaceData = faceData
        };

        // Assert
        Assert.Equal(faceId, face.Id);
        Assert.Equal(orderId, face.OrderId);
        Assert.Equal(faceData, face.FaceData);
    }

    [Fact]
    public void Order_RequiredFields_ShouldThrowValidationException()
    {
        // Arrange
        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            Status = OrderStatus.Registered,
            ImageData = new byte[] { }
        };

        // Act & Assert
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(order, null, null);
        var isValid = Validator.TryValidateObject(order, context, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.ErrorMessage.Contains("The Username field is required."));
        Assert.Contains(validationResults, v => v.ErrorMessage.Contains("The Email field is required."));
    }

    [Fact]
    public void OrderCreateDto_ShouldAssignProperties()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var username = "testUser";
        var email = "test@example.com";
        var imageName = "image.jpg";
        var imageFile = Substitute.For<IFormFile>();

        // Act
        var dto = new OrderCreateDto
        {
            OrderId = orderId,
            Username = username,
            Email = email,
            ImageName = imageName,
            ImageFile = imageFile
        };

        // Assert
        Assert.Equal(orderId, dto.OrderId);
        Assert.Equal(username, dto.Username);
        Assert.Equal(email, dto.Email);
        Assert.Equal(imageName, dto.ImageName);
        Assert.Equal(imageFile, dto.ImageFile);
    }

    [Fact]
    public void OrderStatus_ShouldContainCorrectValues()
    {
        // Act & Assert
        Assert.Equal(0, (int)OrderStatus.Registered);
        Assert.Equal(1, (int)OrderStatus.Processed);
        Assert.Equal(2, (int)OrderStatus.Sent);
    }

    [Fact]
    public void Response_ShouldAssignProperties()
    {
        // Arrange
        var status = "success";
        var result = new { Message = "Operation completed" };

        // Act
        var response = new Response(status, result);

        // Assert
        Assert.Equal(status, response.Status);
        Assert.Equal(result, response.Result);
    }

    [Fact]
    public void User_ShouldHaveCorrectProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "John Doe";
        var username = "johndoe";
        var password = "password123";
        var role = "Admin";

        // Act
        var user = new User(id, name, username, password, role);

        // Assert
        Assert.Equal(id, user.Id);
        Assert.Equal(name, user.Name);
        Assert.Equal(username, user.UserName);
        Assert.Equal(password, user.Password);
        Assert.Equal(role, user.Role);
    }

    [Fact]
    public void UserLoginRequestDto_ShouldAssignProperties()
    {
        // Arrange
        var username = "johndoe";
        var password = "password123";

        // Act
        var dto = new UserLoginRequestDto(username, password);

        // Assert
        Assert.Equal(username, dto.UserName);
        Assert.Equal(password, dto.Password);
    }

    [Fact]
    public void UserLoginResponseDto_ShouldAssignProperties()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "John Doe", "johndoe", "password123", "Admin");
        var token = "sample-token";

        // Act
        var responseDto = new UserLoginResponseDto
        {
            User = user,
            Token = token
        };

        // Assert
        Assert.Equal(user, responseDto.User);
        Assert.Equal(token, responseDto.Token);
    }

    [Fact]
    public void UserRegisterDto_ShouldAssignPropertiesCorrectly()
    {
        // Arrange
        var name = "John Doe";
        var username = "johndoe";
        var password = "password123";

        // Act
        var userRegisterDto = new UserRegisterDto(name, username, password);

        // Assert
        Assert.Equal(name, userRegisterDto.Name);
        Assert.Equal(username, userRegisterDto.UserName);
        Assert.Equal(password, userRegisterDto.Password);
    }

    [Fact]
    public void UserRegisterDto_ShouldBeImmutable()
    {
        // Arrange
        var originalDto = new UserRegisterDto("John Doe", "johndoe", "password123");

        // Act
        var modifiedDto = originalDto with { UserName = "newusername" };

        // Assert
        Assert.NotEqual(originalDto, modifiedDto);
        Assert.Equal("newusername", modifiedDto.UserName);
        Assert.Equal("johndoe", originalDto.UserName); // Original remains unchanged
    }
}