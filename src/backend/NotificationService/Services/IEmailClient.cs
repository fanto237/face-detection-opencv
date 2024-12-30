using MimeKit;

namespace MailService.Services;

public interface IEmailClient
{
    Task SendEmailAsync(MimeMessage email);
}

