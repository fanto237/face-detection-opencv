using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OrderApi.Controllers;
using OrderApi.Mapping;
using OrderApi.Models;
using OrderApi.Repository;
using OrderApi.Services;

namespace OrderApi.Tests.Controllers;

public class OrdersControllerTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IPublishEndpoint _publishEndpoint = Substitute.For<IPublishEndpoint>();
    private readonly ILogger<OrdersController> _logger = Substitute.For<ILogger<OrdersController>>();
    private readonly IFileProcessingService _fileProcessingService = Substitute.For<IFileProcessingService>();

    [Fact]
    public async Task GetById_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var controller = new OrdersController(_orderRepository, _mapper, _publishEndpoint, _logger, _fileProcessingService);

        // Act
        var result = await controller.GetById(Guid.Empty);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_OrderNotFound_ReturnsNotFound()
    {
        // Arrange
        var controller = new OrdersController(_orderRepository, _mapper, _publishEndpoint, _logger, _fileProcessingService);
        var nonExistentOrderId = Guid.NewGuid();
        _orderRepository.GetById(nonExistentOrderId).Returns((Order)null);

        // Act
        var result = await controller.GetById(nonExistentOrderId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_OrderExists_ReturnsOk()
    {
        // Arrange
        var controller = new OrdersController(_orderRepository, _mapper, _publishEndpoint, _logger, _fileProcessingService);
        var existingOrderId = Guid.NewGuid();
        var order = new Order { OrderId = existingOrderId };
        _orderRepository.GetById(existingOrderId).Returns(order);
        var orderGetDto = new OrderGetDto(){OrderId = existingOrderId};
        _mapper.Map<OrderGetDto>(order).Returns(orderGetDto);

        // Act
        var result = await controller.GetById(existingOrderId);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_ValidOrder_ReturnsCreated()
    {
        // Arrange
        var controller = new OrdersController(_orderRepository, _mapper, _publishEndpoint, _logger, _fileProcessingService);
        var newOrder = new OrderCreateDto
        {
            ImageFile = Substitute.For<IFormFile>()
        };
        var createdOrder = new Order { OrderId = Guid.NewGuid() };

        _mapper.Map<Order>(newOrder).Returns(createdOrder);
        _fileProcessingService.ConvertToBytes(newOrder.ImageFile).Returns([1, 2, 3]);
        _fileProcessingService.GenerateImageName(newOrder.ImageFile).Returns("test-image.jpg");

        // Act
        var result = await controller.Create(newOrder);

        // Assert
        Assert.IsType<CreatedAtRouteResult>(result);
    }
}