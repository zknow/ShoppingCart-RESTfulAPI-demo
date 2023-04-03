using Microsoft.EntityFrameworkCore;
using ShoppingCart.Dtos;

namespace ShoppingCart.DB;

public class DbCtx : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public DbCtx(DbContextOptions<DbCtx> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<Customer>().ToTable("Customers");
    }
}