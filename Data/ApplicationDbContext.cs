using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models.Product;
using InventoryManagementSystem.Models.Category;
using InventoryManagementSystem.Models.Supplier;
namespace InventoryManagementSystem.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
}