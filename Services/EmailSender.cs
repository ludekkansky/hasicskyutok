using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace hasicskyutok.Services;
public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = GetSmtpClient();
        var mMessage = new MailMessage()
        {
            From = new MailAddress("test@test.localhost"),
            Subject = subject,
            IsBodyHtml = true,
            Body = htmlMessage,
        };
        mMessage.To.Add(new MailAddress(email));
        return client.SendMailAsync(mMessage);
    }

    public Task SendMailAsync(MailMessage msg)
    {
        var client = GetSmtpClient();
        msg.From = new MailAddress("test@localhost");
        msg.IsBodyHtml = true;
        return client.SendMailAsync(msg);
    }

    public static SmtpClient GetSmtpClient()
    {
        SmtpClient client = new()
        {
            Port = 587,
            Host = "localhost",
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("test@localhost", "emailPassword")
        };

        return client;
    }
}
