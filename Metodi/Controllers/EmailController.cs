using Metodi.Data;
using Metodi.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Metodi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController :  ControllerBase
{
    private readonly AppDbContext _contex;
    private readonly IEmailService _emailService;

    public EmailController(AppDbContext contex, IEmailService emailService)
    {
        _contex = contex;
        _emailService = emailService;
    }

    public async Task<IActionResult> SendOrderEmail(int clientId, int orderId)
    {
        var item = _contex.Items.FirstOrDefault(x => x.Id == orderId);
        
        var client = _contex.Clients.FirstOrDefault(x => x.Id == clientId);
        
        var subject = "Order Information";
        var body = $"Hello {client.Username}, this is your order for '{item.Name}'";
        await _emailService.SendEmailAsync(client.Email, subject, body, orderId);
        
        return Ok($"Order {orderId} has been sent");
    }
}