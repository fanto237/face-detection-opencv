using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models;

public class Order
{
    [Key] [DisplayName("id")] public Guid OrderId { get; set; }

    [Required] public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public OrderStatus Status { get; set; }

    public byte[] ImageData { get; set; }

    // [MaxLength(3)] to specify the maximum amount of items that the list can carry
    public List<Face> Faces { get; set; }
}