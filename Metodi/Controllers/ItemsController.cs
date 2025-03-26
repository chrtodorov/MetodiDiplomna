using Metodi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Metodi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ItemsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        var items = await _context.Items.Where(x => x.IsOrdered == false).ToListAsync();
        return Ok(items);
    }
}
