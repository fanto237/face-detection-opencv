using MailKit.Net.Smtp;
using MailKit.Security;
using MailService.Services;
using MassTransit;
using MimeKit;
using SharedLib.Contracts;

namespace MailService.Consumers;

public class OrderSendConsumer(IPublishEndpoint publishEndpoint, EmailConfig emailConfig, IEmailClient _mailClient) : IConsumer<IOrderSendEvent>
{

    public async Task Consume(ConsumeContext<IOrderSendEvent> context)
    {
        var msg = context.Message;
        var message = new Message(msg.Email, $"Your Order {msg.OrderId}", msg.Faces, msg.UserName);
        var email = await CreateEmail(message);
        await _mailClient.SendEmailAsync(email);

        await publishEndpoint.Publish<IOrderSentEvent>(new
        {
            msg.OrderId,
            DateTime.Now
        });
    }

    private Task<MimeMessage> CreateEmail(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(emailConfig.Name, emailConfig.From));
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
}