using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
}

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.FindAsync(id)
            ?? throw new KeyNotFoundException($"Product with id {id} was not found.");
    }

    public async Task AddProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product is null)
        {
            return;
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }
}