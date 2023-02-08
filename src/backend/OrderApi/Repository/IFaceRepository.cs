using OrderApi.Models;

namespace OrderApi.Repository;

public interface IFaceRepository
{
    Task Create(Face order);
    Task Save();
}