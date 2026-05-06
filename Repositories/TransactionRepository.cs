using InventoryManagementSystem.Models.Transaction;
using System.Collections.Generic;
namespace InventoryManagementSystem.Repositories;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    Task<Transaction> GetTransactionByIdAsync(int id);
    Task AddTransactionAsync(Transaction transaction);
    Task UpdateTransactionAsync(Transaction transaction);
    Task DeleteTransactionAsync(int id);
}
public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        return await _dbContext.Transactions.AsNoTracking().ToListAsync();
    }

    public async Task<Transaction> GetTransactionByIdAsync(int id)
    {
        return await _dbContext.Transactions.FindAsync(id)
            ?? throw new KeyNotFoundException($"Transaction with id {id} was not found.");
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        _dbContext.Transactions.Update(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(int id)
    {
        var transaction = await GetTransactionByIdAsync(id);
        _dbContext.Transactions.Remove(transaction);
        await _dbContext.SaveChangesAsync();
    }
}