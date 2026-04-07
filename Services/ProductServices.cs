using InventoryManagementSystem.Models.Product;
using InventoryManagementSystem.Repositories;
namespace InventoryManagementSystem.Services;
public class ProductServices: IProductRepository
{
    private readonly IProductRepository _productRepository;
    public ProductServices(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllProductsAsync();
    }
    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetProductByIdAsync(id);
    }
    public async Task<IEnumerable<Product>> GetProductByNameAsync(string searchString)
    {
        if(!string.IsNullOrWhiteSpace(searchString))
        {
            return await _productRepository.GetProductByNameAsync(searchString);
        }
        return await _productRepository.GetAllProductsAsync();
    }
    public async Task<IEnumerable<Product>> GetProductByCategoryIdAsync(int id)
    {
        return await _productRepository.GetProductByCategoryIdAsync(id);
    }
    public async Task<IEnumerable<Product>> GetProductBySupplierIdAsync(int id)
    {
        return await _productRepository.GetProductBySupplierIdAsync(id);
    }
    public async Task AddProductAsync(Product product)
    {
        await _productRepository.AddProductAsync(product);
    }
    public async Task UpdateProductAsync(Product product)
    {
        await _productRepository.UpdateProductAsync(product);
    }
    public async Task DeleteProductAsync(int id)
    {
        await _productRepository.DeleteProductAsync(id);
    }
}