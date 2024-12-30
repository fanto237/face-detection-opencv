using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;
using OrderApi.Repository;

namespace OrderApi.Tests.Repositories;

public class OrderRepositoryTests
{
    [Fact]
    public async Task Get_ShouldRetrieveAllOrders()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "OrderDatabase")
            .Options;

        await using var context = new ApplicationDbContext(options);
        context.Orders.Add(new Order { OrderId = Guid.NewGuid(), Username = "user1", Email = "user1@example.com", ImageName = "image1.jpg", ImageData =
            [1, 2, 3]
        });
        context.Orders.Add(new Order { OrderId = Guid.NewGuid(), Username = "user2", Email = "user2@example.com", ImageName = "image2.jpg", ImageData =
            [4, 5, 6]
        });
        await context.SaveChangesAsync();

        var repository = new OrderRepository(context);

        // Act
        var orders = await repository.Get();

        // Assert
        Assert.Equal(2, orders.Count());
    }

    [Fact]
    public async Task GetById_ShouldRetrieveCorrectOrder()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "OrderByIdDatabase")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var orderId = Guid.NewGuid();
        context.Orders.Add(new Order { OrderId = orderId, Username = "user1", Email = "user1@example.com", ImageName = "image.jpg", ImageData =
            [1, 2, 3]
        });
        await context.SaveChangesAsync();

        var repository = new OrderRepository(context);

        // Act
        var order = await repository.GetById(orderId);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(orderId, order.OrderId);
    }

    [Fact]
    public async Task Create_ShouldAddOrderToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CreateOrderDatabase")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var repository = new OrderRepository(context);
        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            Username = "user1",
            Email = "user1@example.com",
            ImageName = "image.jpg",
            ImageData = new byte[] { 1, 2, 3 }
        };

        // Act
        await repository.Create(order);

        // Assert
        Assert.Equal(1, await context.Orders.CountAsync());
        Assert.Equal(order.OrderId, (await context.Orders.FirstAsync()).OrderId);
    }
}