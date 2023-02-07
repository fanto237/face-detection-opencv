namespace SharedLib.Contracts;

public interface IOrderRegisteredEvent
{
    public Guid OrderId { get; set; }
    public byte[] ImageData { get; set; }
    public List<byte[]> FaceData { get; set; }
}