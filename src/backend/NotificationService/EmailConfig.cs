﻿namespace MailService;

public class EmailConfig
{
    public string Name { get; set; } = null!;
    public string From { get; set; } = null!;
    public string SmtpServer { get; set; } = null!;
    public int Port { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}