using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models.Product;
using InventoryManagementSystem.Models.Category;
using InventoryManagementSystem.Models.Supplier;
using InventoryManagementSystem.Models.Location;
using InventoryManagementSystem.Models.Transaction;
namespace InventoryManagementSystem.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}