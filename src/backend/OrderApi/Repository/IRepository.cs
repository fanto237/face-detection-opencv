using OrderApi.Models;

namespace OrderApi.Repository;

public interface IRepository
{
    Task<Order> GetById(Guid id);
    Task<IEnumerable<Order>> GetByEmail(string Email);
    Task<bool> Create(Order order);
}
