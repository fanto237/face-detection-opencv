using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using OrderApi.Data;
using OrderApi.Models;
using OrderApi.Repository;

namespace OrderApi.Tests.Repositories;

public class UserRepositoryTests
{
    [Fact]
    public void IsUniqueUser_ShouldReturnTrueForUniqueUser()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "UniqueUserDatabase")
            .Options;
        using var context = new ApplicationDbContext(options);
        var repository = new UserRepository(context, Substitute.For<IConfiguration>(),Substitute.For<IMapper>());

        // Act
        var isUnique = repository.IsUniqueUser("uniqueUser");

        // Assert
        Assert.True(isUnique);
    }

    [Fact]
    public async Task Register_ShouldAddNewUser()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "RegisterUserDatabase")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var configuration = Substitute.For<IConfiguration>();
        var repository = new UserRepository(context, configuration, Substitute.For<IMapper>());
        var userDto = new UserRegisterDto("John Doe", "johndoe", "password");

        // Act
        var user = await repository.Register(userDto);

        // Assert
        Assert.NotNull(user);
        Assert.Equal("johndoe", user.UserName);
        Assert.Equal(1, await context.Users.CountAsync());
    }

    [Fact]
    public async Task Login_ValidCredentials_ShouldReturnUserAndToken()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Login_ValidCredentials_Database")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var config = Substitute.For<IConfiguration>();
        config["Jwt:Secret"].Returns("This is the super secret key for issuing jwt token"); // Mock JWT secret
        config["Jwt:Issuer"].Returns("TestIssuer");

        var user = new User(Guid.NewGuid(), "John Doe", "johndoe", "password123", "Admin");
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, config, Substitute.For<IMapper>());

        var loginRequest = new UserLoginRequestDto("johndoe", "password123");

        // Act
        var response = await repository.Login(loginRequest);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.User);
        Assert.Equal("johndoe", response.User.UserName);
        Assert.False(string.IsNullOrEmpty(response.Token));
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShouldReturnEmptyTokenAndNullUser()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Login_InvalidCredentials_Database")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var config = Substitute.For<IConfiguration>();
        config["Jwt:Secret"].Returns("This is the super secret key for issuing jwt token"); // Mock JWT secret
        config["Jwt:Issuer"].Returns("TestIssuer");

        var user = new User(Guid.NewGuid(), "John Doe", "johndoe", "password123", "Admin");
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, config, Substitute.For<IMapper>());

        var loginRequest = new UserLoginRequestDto("johndoe", "wrongpassword");

        // Act
        var response = await repository.Login(loginRequest);

        // Assert
        Assert.NotNull(response);
        Assert.Null(response.User);
        Assert.Equal("", response.Token);
    }
}