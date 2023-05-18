using NSubstitute;
using OrderApi.Mapping;
using OrderApi.Models;

namespace OrderApiTests;

public class MapperTest
{
    [Fact]
    public void Map_Should_Convert_OrderCreateDto_To_Order()
    {
        // Arrange
        var map = Substitute.For<IMapper>();
        var id = Guid.NewGuid();
        var orderCreate = new OrderCreateDto()
        {
            OrderId = id,
            Email = "luciensiani@gmail.com",
            Username = "fanto",
            ImageName = "best_image.jpeg",
        };
        var order = new Order(){
            OrderId = id,
            Email = "luciensiani@gmail.com",
            Username = "fanto",
            ImageName = "best_image.jpeg",
        };
        map.Map(Arg.Any<OrderCreateDto>()).ReturnsForAnyArgs(order);
        // Act

        var result = map.Map(order);

        // Assert
        Assert.Equivalent(order, result);
    }
}