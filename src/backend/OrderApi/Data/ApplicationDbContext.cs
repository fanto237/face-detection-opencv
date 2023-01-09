using Microsoft.EntityFrameworkCore;
using OrderApi.Models;

namespace OrderApi.Data;

public class ApplicationDbContext : DbContext
{

    public DbSet<Order> Orders { get; set; }
    public DbSet<Face> Faces { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    { }


}
