using OrderApi.Models;

namespace OrderApi.Mapping;

public interface IMapper
{
    Order Map(OrderCreateDto source);
    OrderGetDto Map(Order source);
}