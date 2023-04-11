using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using MimeKit;
using SharedLib.Contracts;

namespace MailService.Consumers;

internal class OrderSendConsumer : IConsumer<IOrderSendEvent>
{
    private readonly EmailConfig _emailConfig;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderSendConsumer(IPublishEndpoint publishEndpoint, EmailConfig emailConfig)
    {
        _publishEndpoint = publishEndpoint;
        _emailConfig = emailConfig;
    }

    public async Task Consume(ConsumeContext<IOrderSendEvent> context)
    {
        var msg = context.Message;
        var message = new Message(msg.Email, $"Your Order {msg.OrderId}", msg.Faces, msg.UserName);
        await SendEmail(message);
        await _publishEndpoint.Publish<IOrderSentEvent>(new
        {
            msg.OrderId,
            DateTime.Now
        });
    }

    private Task<MimeMessage> CreateEmail(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfig.Name, _emailConfig.From));
        emailMessage.To.Add(MailboxAddress.Parse(message.ToEmail));
        emailMessage.Subject = message.Subject;
        var body = new BodyBuilder();
        body.TextBody = message.Attachments.Any()
            ? $"Hey {message.UserName}, \n\n" +
              "Thanks for requesting for a Face Detection, your Order has been successfully processed by us. \n" +
              "In Attachment you can find the faces images contained in the image you submitted. \n\n" +
              "King regards \n" +
              "Your Meta Vision Team"
            : $"Hey {message.UserName}, \n\n" +
              "Thanks for requesting for a Face Detection, your Order has been successfully processed by us. \n" +
              "Sadly they were no faces images  contained in the image you submitted. \n\n" +
              "King regards \n" +
              "Your Meta Vision Team";

        if (message.Attachments.Any())
        {
            var i = 0;
            foreach (var attachment in message.Attachments) body.Attachments.Add($"attachment-{++i}.jpg", attachment);
        }

        emailMessage.Body = body.ToMessageBody();
        return Task.FromResult(emailMessage);
    }

    private async Task SendEmail(Message message)
    {
        var email = await CreateEmail(message);

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);
        await Console.Out.WriteLineAsync("The Email has been sent");
    }
}