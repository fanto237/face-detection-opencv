using OrderApi.Models;

namespace OrderApi.Repository;

public interface IRepository
{
    Task<Order?> GetById(Guid id);
    Task<bool> Create(Order order);
}