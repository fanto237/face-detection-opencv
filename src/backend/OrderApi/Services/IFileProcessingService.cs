namespace OrderApi.Services;

public interface IFileProcessingService
{
    Task<Byte[]> ConvertToBytes(IFormFile imageFile);
    string GenerateImageName(IFormFile imageFile);
}

