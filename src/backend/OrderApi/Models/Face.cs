using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models;

public class Face
{
    [Key] [Required] [DisplayName("Id")] public Guid Id { get; set; }
    [DisplayName("Order Id")] public Guid OrderId { get; set; }
    public byte[] FaceData { get; set; }
}