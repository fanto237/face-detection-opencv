namespace SharedLib.Contracts;

public interface IOrderProcessedEvent
{
    public Guid OrderId { get; set; }
    public List<byte[]> FaceData { get; set; }
}