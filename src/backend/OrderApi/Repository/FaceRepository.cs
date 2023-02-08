using OrderApi.Data;
using OrderApi.Models;

namespace OrderApi.Repository;

class FaceRepository : IFaceRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FaceRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public async Task Create(Face face)
    {
        await _dbContext.Faces.AddAsync(face);
        await Save();
    }

    public  async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}