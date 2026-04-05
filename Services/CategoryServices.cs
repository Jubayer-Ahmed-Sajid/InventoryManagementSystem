using InventoryManagementSystem.Models.Category;
using InventoryManagementSystem.Repositories;
namespace InventoryManagementSystem.Services;

public class CategoryServices: ICategoryRepository
{
    private readonly ICategoryRepository  _CategoryRepository;
    public CategoryServices(ICategoryRepository categoryRepository)
    {
        _CategoryRepository = categoryRepository;
    }

    public async Task <IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _CategoryRepository.GetAllCategoriesAsync();
        
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        Category category =await _CategoryRepository.GetCategoryByIdAsync(id);
        return category;

    }

    public async Task AddCategoryAsync(Category category)
    {
        await _CategoryRepository.AddCategoryAsync(category);
        
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        await _CategoryRepository.UpdateCategoryAsync(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _CategoryRepository.DeleteCategoryAsync(id);
    }
}