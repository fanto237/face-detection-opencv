using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    public class Order
    {
        [Key]
        [DisplayName("id")]
        public Guid OrderId { get; set; }
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public OrderStatus Status { get; set; }
        public byte[] ImageData { get; set; }
        public List<Face> Faces { get; set; }
    }
}
