using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Models;

public class Order
{
    [Key] [DisplayName("id")] public Guid OrderId { get; set; }

    [Required] public string Username { get; set; } = null!;

    [Required] public string Email { get; set; } = null!;

    [Required] public string ImageName { get; set; } = null!;

    [NotMapped] public IFormFile ImageFile { get; set; }

    public OrderStatus Status { get; set; }

    public byte[] ImageData { get; set; }
    // [Column]
    // public List<Face> Faces { get; set; }
}