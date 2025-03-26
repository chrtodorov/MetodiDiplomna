using MailKit.Security;
using Metodi.Interfaces;
using Metodi.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using QRCoder;

namespace Metodi.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _mailSettings;
    public EmailService(IOptions<EmailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse("metodi1diplomna@abv.bg"));
        email.Subject = subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = body;
        
        byte[] qrCodeBytes = GenerateQrCode("http://localhost:4200/viewOrders");
        builder.Attachments.Add("qrcode.png", qrCodeBytes, ContentType.Parse("image/png"));
        
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    } 
    
    private byte[] GenerateQrCode(string url)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
}
//Test1234!
//
//metodi1diplomna@abv.bg
// 
//4C518CE8145E20005D14D7679F561634BE8B
//smtp.elasticemail.com
//2525