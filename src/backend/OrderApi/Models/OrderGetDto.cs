namespace OrderApi.Models;

public class OrderGetDto
{
    public Guid OrderId { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public byte[] ImageData { get; set; } = null!;
}