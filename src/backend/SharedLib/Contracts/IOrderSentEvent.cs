namespace SharedLib.Contracts;

public interface IOrderSentEvent
{
    public Guid OrderId { get; set; }
}