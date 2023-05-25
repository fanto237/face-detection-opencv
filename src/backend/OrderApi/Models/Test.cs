using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models;

public class Test
{
    [Key]
    public Guid id { get; set; }
    public string Name { get; set; }
}