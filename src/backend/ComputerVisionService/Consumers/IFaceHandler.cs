namespace ComputerVisionService.Consumers;

public interface IFaceHandler
{
    Task<List<byte[]>> ExtractFaces(byte[] imageData);
}