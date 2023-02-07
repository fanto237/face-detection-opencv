namespace SharedLib.Contracts;

public interface IOrderProcessedEvent
{
    public Guid OrderId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    // public List<byte[]> Faces { get; set; }
}