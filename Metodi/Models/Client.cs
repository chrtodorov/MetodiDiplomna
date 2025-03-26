namespace Metodi.Models;

public class Client
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public ICollection<Order> Orders { get; set; }
}