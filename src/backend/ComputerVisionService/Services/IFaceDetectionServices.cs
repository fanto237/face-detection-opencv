namespace ComputerVisionService.Services;

public interface IFaceDetectionServices
{
    Task<List<byte[]>> ExtractFaces(byte[] imageData);
}