namespace Metodi.Dtos;

public class SingleItemResponseDto
{
    public string Name { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public string Description { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal TotalPrice { get; set; }
}