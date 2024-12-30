using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Repository;
using OrderApi.Services;
using SharedLib.Contracts;
using Response = OrderApi.Models.Response;

namespace OrderApi.Controllers;

[ApiController]
[Route("/api/v0/[controller]")]
public class OrdersController(
    IOrderRepository orderRepository,
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    ILogger<OrdersController> logger,
    IFileProcessingService fileProcessingService)
    : Controller
{
    private Response?  _response;

    [HttpGet("{id:guid}", Name = "GetOrder")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Response>> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            _response = new Response("error","Invalid id");
            logger.LogError("Invalid id");
            return BadRequest(_response);
        }

        var order = await orderRepository.GetById(id);
        if (order == null)
        {
            _response = new Response("error","Order not found");
            logger.LogError("Order not found");
            return NotFound(_response);
        }
        _response = new Response("success", mapper.Map<OrderGetDto>(order));

        return Ok(_response); ;
    }

    [HttpPost]
    [Authorize(Roles = "Custom")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromForm] OrderCreateDto model)
    {
        var order = mapper.Map<Order>(model);
        order.ImageData = await fileProcessingService.ConvertToBytes(order.ImageFile);
        order.ImageName = fileProcessingService.GenerateImageName(order.ImageFile);
        await orderRepository.Create(order);
        await publishEndpoint.Publish<IOrderRegisteredEvent>(new
        {
            order.OrderId,
            order.ImageData
        });
        _response = new Response("success", order);
        return CreatedAtRoute("GetOrder", new { id = order.OrderId }, _response);
    }


}