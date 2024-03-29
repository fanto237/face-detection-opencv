﻿using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Contracts;

namespace ComputerVisionService.Consumers;

public class OrderRegisteredConsumer : IConsumer<IOrderRegisteredEvent>
{
    private readonly IFaceHandler _faceHandler;
    private readonly ILogger<OrderRegisteredConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderRegisteredConsumer(ILogger<OrderRegisteredConsumer> logger, IPublishEndpoint publishEndpoint,
        IFaceHandler faceHandler)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _faceHandler = faceHandler;
    }

    public async Task Consume(ConsumeContext<IOrderRegisteredEvent> context)
    {
        var msg = context.Message;
        msg.FaceData = await _faceHandler.ExtractFaces(msg.ImageData);
        await _publishEndpoint.Publish<IOrderProcessedEvent>(new
        {
            msg.OrderId,
            msg.FaceData
        });

        _logger.LogInformation($"the new order contains: {msg.FaceData.Count} faces");
    }
}