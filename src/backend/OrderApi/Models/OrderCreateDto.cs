using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Models;

public class OrderCreateDto
{
    [Required] public Guid OrderId { get; set; }

    [Required] public string Username { get; set; } = null!;

    [Required] public string Email { get; set; } = null!;

    [Required] public string ImageName { get; set; } = null!;

    [NotMapped] public IFormFile imageFile { get; set; }
}