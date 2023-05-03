namespace MailService.Consumers;

public class Message
{
    public Message(string toEmail, string subject, List<byte[]> attachments, string userName)
    {
        ToEmail = toEmail;
        Subject = subject;
        Attachments = attachments;
        UserName = userName;
    }

    public string UserName { get; set; }
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public List<byte[]> Attachments { get; set; }
}