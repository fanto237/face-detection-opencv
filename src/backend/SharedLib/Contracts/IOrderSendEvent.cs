namespace SharedLib.Contracts;

public interface IOrderSendEvent
{
    public Guid OrderId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string ImageName { get; set; }
    public List<byte[]> Faces { get; set; }
}