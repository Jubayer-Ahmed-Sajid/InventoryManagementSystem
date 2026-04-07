using InventoryManagementSystem.Models.Supplier;
using InventoryManagementSystem.Repositories;

namespace InventoryManagementSystem.Services;
public class SupplierServices: ISupplierRepository
{
    private readonly ISupplierRepository _SupplierRepository;
    public SupplierServices (ISupplierRepository supplierRepository)
    {
        _SupplierRepository = supplierRepository;
    }

    public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
    {
        var suppliers = await _SupplierRepository.GetAllSuppliersAsync();
        return suppliers;
    }

    public async Task<Supplier> GetSupplierByIdAsync(int id)
    {
        var supplier = await _SupplierRepository.GetSupplierByIdAsync(id);
        return supplier;
    }
 
    public async Task AddSupplierAsync(Supplier supplier)
    {
        await _SupplierRepository.AddSupplierAsync(supplier);
    }

    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        await _SupplierRepository.UpdateSupplierAsync(supplier);
    }

    public async Task DeleteSupplierAsync(int id)
    {
        await _SupplierRepository.DeleteSupplierAsync(id);
    }
}