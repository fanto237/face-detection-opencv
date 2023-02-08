using OrderApi.Models;

namespace OrderApi.Repository;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> Get();
    Task<Order?> GetById(Guid id);
    Task Create(Order order);
    Task Save();
}