using InventoryManagementSystem.Models.Supplier;
using System.Collections.Generic;  
namespace InventoryManagementSystem.Repositories;
public interface ISupplierRepository
{
    Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
    Task<Supplier> GetSupplierByIdAsync(int id);
    Task AddSupplierAsync(Supplier supplier);
    Task UpdateSupplierAsync(Supplier supplier);
    Task DeleteSupplierAsync(int id);
}