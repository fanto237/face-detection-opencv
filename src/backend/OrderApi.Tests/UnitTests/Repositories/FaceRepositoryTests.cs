using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;
using OrderApi.Repository;

namespace OrderApi.Tests.Repositories;

public class FaceRepositoryTests
{
    [Fact]
    public async Task Create_ShouldAddFaceToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "FaceDatabase")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var repository = new FaceRepository(context);
        var face = new Face
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            FaceData = new byte[] { 1, 2, 3 }
        };

        // Act
        await repository.Create(face);

        // Assert
        Assert.Equal(1, await context.Faces.CountAsync());
        Assert.Equal(face.Id, (await context.Faces.FirstAsync()).Id);
    }

    [Fact]
    public async Task Save_ShouldPersistChanges()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "SaveFaceDatabase")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var repository = new FaceRepository(context);
        var face = new Face
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            FaceData = new byte[] { 1, 2, 3 }
        };

        // Act
        context.Faces.Add(face);
        await repository.Save();

        // Assert
        Assert.Equal(1, await context.Faces.CountAsync());
    }
}