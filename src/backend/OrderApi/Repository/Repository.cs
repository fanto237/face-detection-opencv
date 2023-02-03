using OrderApi.Data;
using OrderApi.Models;

namespace OrderApi.Repository;

public class Repository : IRepository
{
    private readonly ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Order?> GetById(Guid id)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        return order;
    }


    public async Task<bool> Create(Order order)
    {
        await _dbContext.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}