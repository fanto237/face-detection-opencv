using MailKit.Net.Smtp;
using MailKit.Security;
using MailService;
using MailService.Consumers;
using MailService.Services;
using MassTransit;
using MimeKit;
using NSubstitute;
using SharedLib.Contracts;

namespace NotificationService.Tests.Consumers;

public class OrderSendConsumerTests
{
    [Fact]
    public async Task Consume_ShouldSendEmailAndPublishEvent()
    {
        // Arrange
        var publishEndpoint = Substitute.For<IPublishEndpoint>();
        var emailClient = Substitute.For<IEmailClient>();
        var emailConfig = new EmailConfig
        {
            From = "no-reply@metavision.com",
            Name = "Meta Vision",
            SmtpServer = "smtp.metavision.com",
            Port = 587,
            UserName = "smtp-user",
            Password = "smtp-pass"
        };
        var consumer = new OrderSendConsumer(publishEndpoint, emailConfig, emailClient);

        var context = Substitute.For<ConsumeContext<IOrderSendEvent>>();
        var faces = new List<byte[]> { new byte[] { 1, 2, 3 } };
        var orderSendEvent = Substitute.For<IOrderSendEvent>();
        orderSendEvent.OrderId.Returns(Guid.NewGuid());
        orderSendEvent.Email.Returns("user@example.com");
        orderSendEvent.Faces.Returns(faces);
        orderSendEvent.UserName.Returns("JohnDoe");
        context.Message.Returns(orderSendEvent);

        // Act
        await consumer.Consume(context);

        // Assert
        // Verify email was sent
        await emailClient.Received(1).SendEmailAsync(Arg.Any<MimeMessage>());

        // Verify event was published
        // await publishEndpoint.Received(1).Publish<IOrderSentEvent>(Arg.Is<object>(e =>
        //     e.GetType().GetProperty("OrderId").GetValue(e).Equals(context.Message.OrderId) &&
        //     e.GetType().GetProperty("DateTime").GetValue(e) is DateTime
        // ));
        await publishEndpoint.Received(1).Publish<IOrderSentEvent>(Arg.Any<object>());
    }

    [Fact]
    public async Task Consume_ShouldHandleEmptyFacesGracefully()
    {
        // Arrange
        var publishEndpoint = Substitute.For<IPublishEndpoint>();
        var emailClient = Substitute.For<IEmailClient>();
        var emailConfig = new EmailConfig
        {
            From = "no-reply@metavision.com",
            Name = "Meta Vision",
            SmtpServer = "smtp.metavision.com",
            Port = 587,
            UserName = "smtp-user",
            Password = "smtp-pass"
        };

        var consumer = new OrderSendConsumer(publishEndpoint, emailConfig, emailClient);

        var context = Substitute.For<ConsumeContext<IOrderSendEvent>>();
        var orderSendEvent = Substitute.For<IOrderSendEvent>();
        orderSendEvent.OrderId.Returns(Guid.NewGuid());
        orderSendEvent.Email.Returns("user@example.com");
        orderSendEvent.Faces.Returns(new List<byte[]>());
        orderSendEvent.UserName.Returns("JohnDoe");
        context.Message.Returns(orderSendEvent);


        // Act
        await consumer.Consume(context);

        // Assert
        // Verify email was sent with no attachments
        await emailClient.Received(1).SendEmailAsync(Arg.Is<MimeMessage>(msg =>
            msg.To.ToString() == "user@example.com" &&
            msg.Subject.Contains("Your Order") &&
            msg.Body.ToString().Contains("no faces images")
        ));

        // Verify event was published
        await publishEndpoint.Received(1).Publish<IOrderSentEvent>(Arg.Any<object>());
    }
}