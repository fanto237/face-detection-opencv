namespace OrderApi.Models;

public class OrderCreateDto
{
    public Guid OrderId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ImageName { get; set; } = null!;
    public IFormFile ImageFile { get; set; }
}