using AutoMapper;
using OrderApi.Models;

namespace OrderApi.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<OrderCreateDto, Order>().ReverseMap();
    }
}