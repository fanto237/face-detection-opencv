using OrderApi.Models;

namespace OrderApi.Mapping;

public class Mapper : IMapper
{
    public Order Map<T>(T source) where T : OrderCreateDto, new()
    {
        return Map(source, new Order());
    }

    private Order Map(OrderCreateDto source, Order destination)
    {
        destination.OrderId = source.OrderId;
        destination.Username = source.Username;
        destination.Email = source.Email;
        destination.ImageName = source.ImageName;
        destination.ImageFile = source.imageFile;
        return destination;
    }
}