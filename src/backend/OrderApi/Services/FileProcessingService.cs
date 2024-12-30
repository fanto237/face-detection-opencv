namespace OrderApi.Services;

public class FileProcessingService : IFileProcessingService
{

    public async Task<byte[]> ConvertToBytes(IFormFile imageFile)
    {
        using var ms = new MemoryStream();
        await imageFile.CopyToAsync(ms);
        var byteList = ms.ToArray();

        return byteList;
    }

    string IFileProcessingService.GenerateImageName(IFormFile imageFile)
    {
        var imgName =
            new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(" ", "-");
        imgName = imgName + DateTime.Now.ToString("yy-MM-dd") + Path.GetExtension(imageFile.FileName);
        return imgName;
    }
}