using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using MimeKit;

namespace MailService.Services;

public class EmailClient(EmailConfig emailConfig) : IEmailClient
{
    public async Task SendEmailAsync(MimeMessage email)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);
    }
}