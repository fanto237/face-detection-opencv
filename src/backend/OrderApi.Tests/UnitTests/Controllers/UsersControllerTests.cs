using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OrderApi.Controllers;
using OrderApi.Models;
using OrderApi.Repository;

namespace OrderApi.Tests.Controllers;

public class UsersControllerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ILogger<UsersController> _logger = Substitute.For<ILogger<UsersController>>();

    [Fact]
    public async Task Register_UniqueUser_ReturnsOk()
    {
        // Arrange
        var controller = new UsersController(_userRepository, _logger);
        var newUser = new UserRegisterDto(string.Empty, UserName: "uniqueUser", string.Empty);
        _userRepository.IsUniqueUser(newUser.UserName).Returns(true);
        _userRepository.Register(newUser).Returns(new User(Guid.Empty,string.Empty, newUser.UserName,string.Empty, string.Empty));

        // Act
        var result = await controller.Register(newUser);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Register_UserAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var controller = new UsersController(_userRepository, _logger);
        var existingUser = new UserRegisterDto(string.Empty,"existingUser", string.Empty);
        _userRepository.IsUniqueUser(existingUser.UserName).Returns(false);

        // Act
        var result = await controller.Register(existingUser);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var controller = new UsersController(_userRepository, _logger);
        var loginRequest = new UserLoginRequestDto("validUser", "password");
        _userRepository.Login(loginRequest).Returns(new UserLoginResponseDto
        {
            User = new User (Guid.Empty, string.Empty, loginRequest.UserName, string.Empty, string.Empty),
            Token = "validToken"
        });

        // Act
        var result = await controller.Login(loginRequest);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsBadRequest()
    {
        // Arrange
        var controller = new UsersController(_userRepository, _logger);
        var loginRequest = new UserLoginRequestDto( "invalidUser", "wrongPassword");
        _userRepository.Login(loginRequest).Returns((UserLoginResponseDto)null);

        // Act
        var result = await controller.Login(loginRequest);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}