using Metodi.Data;
using Metodi.Dtos;
using Metodi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Metodi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existing = await _context.Clients.FirstOrDefaultAsync(c => c.Username == dto.Username);
        if (existing != null)
            return BadRequest("Username already exists.");

        var client = new Client { Username = dto.Username, Password = dto.Password, Email = dto.Email };
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return Ok(new { ClientId = client.Id, Message = "Registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Username == dto.Username && c.Password == dto.Password);
        if (client == null) return Unauthorized("Invalid credentials.");

        return Ok(new { ClientId = client.Id, Message = "Login successful." });
    }
}