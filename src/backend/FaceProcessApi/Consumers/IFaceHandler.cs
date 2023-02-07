namespace FaceProcessApi.Consumers;

public interface IFaceHandler
{
    List<byte[]> ExtractFaces(byte[] imageData);
}