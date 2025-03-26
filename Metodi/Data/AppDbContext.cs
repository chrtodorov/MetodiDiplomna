using Metodi.Models;
using Microsoft.EntityFrameworkCore;

namespace Metodi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().ToTable("clients");
        
        modelBuilder.Entity<Client>()
            .Property(c => c.Id)
            .HasColumnName("id");  // Column name for 'Id'

        modelBuilder.Entity<Client>()
            .Property(c => c.Username)
            .HasColumnName("username");  // Column name for 'Username'

        modelBuilder.Entity<Client>()
            .Property(c => c.Password)
            .HasColumnName("password"); 
        
        modelBuilder.Entity<Client>()
            .Property(c => c.Email)
            .HasColumnName("email"); 
        
        modelBuilder.Entity<Item>()
            .ToTable("items", "public");  // Ensure table name is lowercase
        
        modelBuilder.Entity<Item>()
            .Property(i => i.Id)
            .HasColumnName("id");  // Column name in lowercase
        
        modelBuilder.Entity<Item>()
            .Property(i => i.Name)
            .HasColumnName("name");  // Column name in lowercase
        
        modelBuilder.Entity<Item>()
            .Property(i => i.Description)
            .HasColumnName("description"); 
        
        modelBuilder.Entity<Item>()
            .Property(i => i.Price)
            .HasColumnName("price");

        modelBuilder.Entity<Item>()
            .Property(i => i.IsOrdered)
            .HasColumnName("is_ordered");


        // Orders Table
        modelBuilder.Entity<Order>()
            .ToTable("orders", "public");  // Ensure table name is lowercase
        
        modelBuilder.Entity<Order>()
            .Property(o => o.Id)
            .HasColumnName("id");  // Column name in lowercase
        
        modelBuilder.Entity<Order>()
            .Property(o => o.ClientId)
            .HasColumnName("clientid");  // Column name in lowercase
        
        modelBuilder.Entity<Order>()
            .Property(o => o.ItemId)
            .HasColumnName("itemid");  // Column name in lowercase
        
        modelBuilder.Entity<Order>()
            .Property(o => o.OrderDate)
            .HasColumnName("orderdate");
        
        modelBuilder.Entity<Order>()
            .Property(o => o.Quantity)
            .HasColumnName("quantity");
        
        modelBuilder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasColumnName("totalprice");
        
        base.OnModelCreating(modelBuilder);
    }
}