namespace Metodi.Models;

public class Order
{
    public int Id { get; set; }
    
    public int ClientId { get; set; }
    
    public Client? Client { get; set; }
    
    public int ItemId { get; set; }
    
    public Item? Item { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal TotalPrice { get; set; }
}