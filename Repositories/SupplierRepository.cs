using InventoryManagementSystem.Models.Supplier;
using System.Collections.Generic;  
namespace InventoryManagementSystem.Repositories;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
public interface ISupplierRepository
{
    Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
    Task<Supplier> GetSupplierByIdAsync(int id);
    Task AddSupplierAsync(Supplier supplier);
    Task UpdateSupplierAsync(Supplier supplier);
    Task DeleteSupplierAsync(int id);
}
public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SupplierRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
    {
        return await _dbContext.Suppliers.AsNoTracking().ToListAsync();
    }

    public async Task<Supplier> GetSupplierByIdAsync(int id)
    {
        return await _dbContext.Suppliers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Supplier with id {id} was not found.");
    }

    public async Task AddSupplierAsync(Supplier supplier)
    {
        _dbContext.Suppliers.Add(supplier);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        _dbContext.Suppliers.Update(supplier);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteSupplierAsync(int id)
    {
        var supplier = await _dbContext.Suppliers.FindAsync(id);
        if (supplier is null)
        {
            return;
        }

        _dbContext.Suppliers.Remove(supplier);
        await _dbContext.SaveChangesAsync();
    }
}