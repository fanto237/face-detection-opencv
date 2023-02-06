using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Repository;

namespace OrderApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class OrderController : Controller
{
    private readonly IRepository _repository;

    public OrderController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> Get()
    {
        Console.WriteLine("get all is been called");
        return await _repository.Get();
    }
    
    [HttpGet("{id:guid}", Name = "GetOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> GetById(Guid id)
    {
        if (id == Guid.Empty)
            // todo add logging
            return BadRequest();

        var order = await _repository.GetById(id);
        return order;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromForm] Order order)
    {
        order.ImageData = await ConvertToByte(order.imageFile);
        order.ImageName = GenerateImageName(order.imageFile);
        order.Status = OrderStatus.Registered;
        await _repository.Create(order);
        // todo produce an event that the order has been registered and process it to rabbitmq
        return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
    }


    [NonAction]
    private static async Task<byte[]> ConvertToByte(IFormFile imageFile)
    {
        using var ms = new MemoryStream();
        await imageFile.CopyToAsync(ms);
        var byteList = ms.ToArray();

        return byteList;
    }

    [NonAction]
    private static string GenerateImageName(IFormFile imageFile)
    {
        var imgName =
            new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(" ", "-");
        imgName = imgName + DateTime.Now.ToString("yy-MM-dd") + Path.GetExtension(imageFile.FileName);
        return imgName;
    }


    // todo publish
    // [NonAction]
    // private static async Task PublishCommand()
    // {
    //     
    // }
}