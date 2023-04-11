using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Mapping;
using OrderApi.Models;
using OrderApi.Repository;
using SharedLib.Contracts;

namespace OrderApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class OrdersController : Controller
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrdersController(IOrderRepository orderRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet("{id:guid}", Name = "GetOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderGetDto>> GetById(Guid id)
    {
        if (id == Guid.Empty)
            // todo add logging
            return BadRequest();

        var order = await _orderRepository.GetById(id);
        var result = _mapper.Map(order);
        return result;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromForm] OrderCreateDto model)
    {
        var order = _mapper.Map(model);
        order.ImageData = await ConvertToByte(order.ImageFile);
        order.ImageName = GenerateImageName(order.ImageFile);
        await _orderRepository.Create(order);
        await _publishEndpoint.Publish<IOrderRegisteredEvent>(new
        {
            order.OrderId,
            order.ImageData
        });
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
    [NonAction]
    private static async Task PublishCommand(Order order)
    {
    }
}