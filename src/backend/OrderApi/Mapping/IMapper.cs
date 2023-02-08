using OrderApi.Models;

namespace OrderApi.Mapping;

public interface IMapper
{
    Order Map<T>(T model) where T : OrderCreateDto, new();
}