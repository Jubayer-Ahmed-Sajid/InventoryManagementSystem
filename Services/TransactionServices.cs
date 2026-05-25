using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models.Location;
using InventoryManagementSystem.Models.Product;
using InventoryManagementSystem.Models.Transaction;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Repositories;

public class TransactionServices
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILocationRepository _locationRepository;

    public TransactionServices(
        ApplicationDbContext dbContext,
        ITransactionRepository transactionRepository,
        ILocationRepository locationRepository)
    {
        _dbContext = dbContext;
        _transactionRepository = transactionRepository;
        _locationRepository = locationRepository;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        return await _transactionRepository.GetAllTransactionsAsync();
    }

    public async Task<Transaction> GetTransactionByIdAsync(int id)
    {
        return await _transactionRepository.GetTransactionByIdAsync(id);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await NormalizeTransactionAsync(transaction);

        if (transaction.Quantity <= 0)
        {
            throw new InvalidOperationException("Transaction quantity must be greater than zero.");
        }

        await using var databaseTransaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == transaction.ProductId)
                ?? throw new KeyNotFoundException($"Product with id {transaction.ProductId} was not found.");

            var updatedQuantity = product.Quantity + GetStockDelta(transaction);
            EnsureQuantityIsValid(updatedQuantity, product.Id);
            product.Quantity = updatedQuantity;

            if (transaction.Date == default)
            {
                transaction.Date = DateTime.UtcNow;
            }

            if (string.IsNullOrWhiteSpace(transaction.TransactionID))
            {
                transaction.TransactionID = $"TXN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
            }

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();
            await databaseTransaction.CommitAsync();
        }
        catch
        {
            await databaseTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        await NormalizeTransactionAsync(transaction);

        if (transaction.Quantity <= 0)
        {
            throw new InvalidOperationException("Transaction quantity must be greater than zero.");
        }

        await using var databaseTransaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var existingTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id)
                ?? throw new KeyNotFoundException($"Transaction with id {transaction.Id} was not found.");

            var oldDelta = GetStockDelta(existingTransaction);
            var newDelta = GetStockDelta(transaction);

            if (existingTransaction.ProductId == transaction.ProductId)
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == transaction.ProductId)
                    ?? throw new KeyNotFoundException($"Product with id {transaction.ProductId} was not found.");

                var updatedQuantity = product.Quantity - oldDelta + newDelta;
                EnsureQuantityIsValid(updatedQuantity, product.Id);
                product.Quantity = updatedQuantity;
            }
            else
            {
                var oldProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == existingTransaction.ProductId)
                    ?? throw new KeyNotFoundException($"Product with id {existingTransaction.ProductId} was not found.");
                var newProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == transaction.ProductId)
                    ?? throw new KeyNotFoundException($"Product with id {transaction.ProductId} was not found.");

                var revertedOldQuantity = oldProduct.Quantity - oldDelta;
                var appliedNewQuantity = newProduct.Quantity + newDelta;

                EnsureQuantityIsValid(revertedOldQuantity, oldProduct.Id);
                EnsureQuantityIsValid(appliedNewQuantity, newProduct.Id);

                oldProduct.Quantity = revertedOldQuantity;
                newProduct.Quantity = appliedNewQuantity;
            }

            existingTransaction.ProductId = transaction.ProductId;
            existingTransaction.Quantity = transaction.Quantity;
            existingTransaction.TransactionType = transaction.TransactionType;
            existingTransaction.LocationFromId = transaction.LocationFromId;
            existingTransaction.LocationToId = transaction.LocationToId;
            existingTransaction.Date = transaction.Date == default ? DateTime.UtcNow : transaction.Date;
            existingTransaction.TransactionID = string.IsNullOrWhiteSpace(transaction.TransactionID)
                ? existingTransaction.TransactionID
                : transaction.TransactionID;

            await _dbContext.SaveChangesAsync();
            await databaseTransaction.CommitAsync();
        }
        catch
        {
            await databaseTransaction.RollbackAsync();
            throw;
        }
    }

    private async Task NormalizeTransactionAsync(Transaction transaction)
    {
        var warehouseId = await GetWarehouseLocationIdAsync();

        switch (transaction.TransactionType)
        {
            case TransactionType.StockIn:
                if (transaction.LocationFromId <= 0)
                {
                    throw new InvalidOperationException("Please select a supplier/location for stock in.");
                }

                transaction.LocationToId = warehouseId;
                break;

            case TransactionType.StockOut:
                if (transaction.LocationToId <= 0)
                {
                    throw new InvalidOperationException("Please select a customer/location for stock out.");
                }

                transaction.LocationFromId = warehouseId;
                break;

            default:
                throw new InvalidOperationException($"Unsupported transaction type: {transaction.TransactionType}");
        }

        await _locationRepository.GetLocationByIdAsync(transaction.LocationFromId);
        await _locationRepository.GetLocationByIdAsync(transaction.LocationToId);
    }

    private async Task<int> GetWarehouseLocationIdAsync()
    {
        var warehouse = (await _locationRepository.GetLocationsByLocationTypeAsync(LocationType.warehouse)).FirstOrDefault();
        return warehouse?.Id ?? throw new InvalidOperationException("Warehouse location is not configured.");
    }

    private static int GetStockDelta(Transaction transaction)
    {
        return transaction.TransactionType switch
        {
            TransactionType.StockIn => transaction.Quantity,
            TransactionType.StockOut => -transaction.Quantity,
            _ => throw new InvalidOperationException($"Unsupported transaction type: {transaction.TransactionType}")
        };
    }

    private static void EnsureQuantityIsValid(int quantity, int productId)
    {
        if (quantity < 0)
        {
            throw new InvalidOperationException($"Product {productId} does not have enough stock for this transaction.");
        }
    }
}