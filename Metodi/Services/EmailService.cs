using MailKit.Security;
using Metodi.Interfaces;
using Metodi.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using QRCoder;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;

namespace Metodi.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _mailSettings;
    public EmailService(IOptions<EmailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, int orderId)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(toEmail)); // Use dynamic recipient, not a fixed one
        email.Subject = subject;
        
        var builder = new BodyBuilder();
        builder.HtmlBody = body;

        // Generate and attach PDF with order details and QR code
        byte[] pdfBytes = GeneratePdfForOrder(orderId);
        builder.Attachments.Add("orderDetails.pdf", pdfBytes, ContentType.Parse("application/pdf"));
        
        email.Body = builder.ToMessageBody();
        
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }

    private byte[] GeneratePdfForOrder(int orderId)
    {
        // Create the PDF document
        var pdf = new PdfDocument();
        var page = pdf.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Arial", 12);

        // Write order details to the PDF
        gfx.DrawString($"Order ID: {orderId}", font, XBrushes.Black, new XPoint(20, 20));
        gfx.DrawString("Order Details", font, XBrushes.Black, new XPoint(20, 40));
        gfx.DrawString("Product Name: Example Product", font, XBrushes.Black, new XPoint(20, 60));
        gfx.DrawString("Quantity: 1", font, XBrushes.Black, new XPoint(20, 80));
        gfx.DrawString("Total Price: 100.00 лв.", font, XBrushes.Black, new XPoint(20, 100));

        // Generate the QR code as an image and embed it into the PDF
        byte[] qrCodeBytes = GenerateQrCode($"http://localhost:4200/myorder/{orderId}");
        using (var ms = new MemoryStream(qrCodeBytes))
        {
            var qrImage = XImage.FromStream(() => ms); // Load the QR code image from memory stream
            gfx.DrawImage(qrImage, 20, 120, 100, 100); // Draw the QR code image at the specified position and size
        }

        // Convert PDF to byte array and return it
        using (var ms = new MemoryStream())
        {
            pdf.Save(ms);
            return ms.ToArray();
        }
    }

    // Generate QR code as byte array
    private byte[] GenerateQrCode(string url)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
}
