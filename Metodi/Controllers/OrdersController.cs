using Metodi.Data;
using Metodi.Dtos;
using Metodi.Interfaces;
using Metodi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Metodi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public OrdersController(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost("order")]
    public async Task<IActionResult> PlaceOrder(OrderRequestDto dto)
    {
        var client = await _context.Clients.FindAsync(dto.ClientId);
        var item = await _context.Items.FindAsync(dto.ItemId);
        if (client == null || item == null)
            return NotFound("Client or Item not found.");

        var order = new Order
        {
            ClientId = dto.ClientId,
            ItemId = dto.ItemId,
            OrderDate = DateTime.UtcNow,
            Quantity = 1,
            TotalPrice = item.Price
        };
        _context.Orders.Add(order);
        item.IsOrdered = true;
        await _context.SaveChangesAsync();

        var subject = "Order Confirmation";
        var body = $"Hello {client.Username}, your order for '{item.Name}' has been placed!";
        await _emailService.SendEmailAsync(client.Email, subject, body, order.Id);

        return Ok("Order placed & email sent.");
    }

    [HttpGet("myorders/{clientId}")]
    public async Task<IActionResult> GetClientOrders(int clientId)
    {
        var orders = await _context.Orders
            .Where(o => o.ClientId == clientId)
            .Include(o => o.Item)
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("myorder/{orderId}")]
    public async Task<IActionResult> GetOrderByOrderId(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        
        var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == order.ItemId);

        var response = new SingleItemResponseDto
        {
            Name = item.Name,
            OrderDate = order.OrderDate,
            Description = item.Description,
            Quantity = order.Quantity,
            TotalPrice = order.TotalPrice,
        };
        
        return Ok(response);
    }
}