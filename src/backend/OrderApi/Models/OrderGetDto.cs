namespace OrderApi.Models;

public class OrderGetDto
{
    public Guid OrderId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public OrderStatus Status { get; set; }
    public byte[] ImageData { get; set; }
}