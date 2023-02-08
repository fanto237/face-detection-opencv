namespace SharedLib.Contracts;

public interface IOrderSentEvent
{
    public Guid OrderId { get; set; }
    public DateTime SentTime { get; set; }
}