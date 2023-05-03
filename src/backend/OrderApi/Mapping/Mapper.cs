using OrderApi.Models;

namespace OrderApi.Mapping;

public class Mapper : IMapper
{
    public Order Map(OrderCreateDto source)
    {
        return Map(source, new Order());
    }

    public OrderGetDto Map(Order source)
    {
        return Map(source, new OrderGetDto());
    }

    private static Order Map(OrderCreateDto source, Order destination)
    {
        destination.OrderId = source.OrderId;
        destination.Username = source.Username;
        destination.Email = source.Email;
        destination.ImageName = source.ImageName;
        destination.ImageFile = source.ImageFile;
        destination.Status = OrderStatus.Registered;
        return destination;
    }

    private static OrderGetDto Map(Order source, OrderGetDto destination)
    {
        destination.OrderId = source.OrderId;
        destination.UserName = source.Username;
        destination.Email = source.Email;
        destination.ImageData = source.ImageData;
        destination.Status = source.Status;
        return destination;
    }
}