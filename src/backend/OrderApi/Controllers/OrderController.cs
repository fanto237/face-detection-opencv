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

    [HttpGet("{id:guid}", Name = "GetOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            // todo add logging
            return BadRequest();
        }

        var order = await _repository.GetById(id);
        if (order is null)
        {
            return NotFound();
        }
        else
        {
            return Ok(order);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Order>>> GetByEmail(string email)
    {
        if (email == null)
        {
            return BadRequest();
        }

        var orders = await _repository.GetByEmail(email);

        if (orders.Count() is 0)
        {
            return NotFound(orders);
        }

        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Order order)
    {
        order.OrderId = Guid.NewGuid();
        order.Status = OrderStatus.Registered;
        var result = await _repository.Create(order);

        return Ok(order);
    }
}
