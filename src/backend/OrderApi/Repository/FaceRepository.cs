using OrderApi.Data;
using OrderApi.Models;

namespace OrderApi.Repository;

public class FaceRepository(ApplicationDbContext dbContext) : IFaceRepository
{
    public async Task Create(Face face)
    {
        await dbContext.Faces.AddAsync(face);
        await Save();
    }

    public async Task Save()
    {
        await dbContext.SaveChangesAsync();
    }
}