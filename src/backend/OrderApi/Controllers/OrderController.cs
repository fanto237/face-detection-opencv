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
    public async Task<ActionResult> Create([FromBody] Order order)
    {
        order.OrderId = Guid.NewGuid();
        order.Status = OrderStatus.Registered;
        var result = await _repository.Create(order);
        // todo produce an event that the order has been registered and process it to rabbitmq
        return Ok(order);
    }
}