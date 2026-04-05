using InventoryManagementSystem.Models.Category;
using System.Collections.Generic;
namespace InventoryManagementSystem.Repositories;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
}
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _dbContext.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _dbContext.Categories.FindAsync(id)
            ?? throw new KeyNotFoundException($"Category with id {id} was not found.");
    }

    public async Task AddCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            return;
        }

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }
}