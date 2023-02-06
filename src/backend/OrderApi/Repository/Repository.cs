using Microsoft.EntityFrameworkCore;
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


    public async Task<IEnumerable<Order>> Get()
    {
        var orders = await _dbContext.Orders.ToListAsync();
        return orders;
    }

    public async Task<Order?> GetById(Guid id)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        return order;
    }

    public async Task Create(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}