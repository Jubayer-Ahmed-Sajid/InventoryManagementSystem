using InventoryManagementSystem.Models.Transaction;
using System.Collections.Generic;
namespace InventoryManagementSystem.Repositories;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

public class TransactionServices
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILocationRepository _locationRepository;

    public TransactionServices(ITransactionRepository transactionRepository, ILocationRepository locationRepository)
    {
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
        // Validate that the locations exist
        var locationFrom = await _locationRepository.GetLocationByIdAsync(transaction.LocationFromId);
        var locationTo = await _locationRepository.GetLocationByIdAsync(transaction.LocationToId);

        if (locationFrom == null)
            throw new KeyNotFoundException($"Location with id {transaction.LocationFromId} was not found.");

        if (locationTo == null)
            throw new KeyNotFoundException($"Location with id {transaction.LocationToId} was not found.");

        // Additional business logic can be added here (e.g., checking stock levels)

        await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        var locationFrom = await _locationRepository.GetLocationByIdAsync(transaction.LocationFromId);
        var locationTo = await _locationRepository.GetLocationByIdAsync(transaction.LocationToId);

        if (locationFrom == null)
            throw new KeyNotFoundException($"Location with id {transaction.LocationFromId} was not found.");

        if (locationTo == null)
            throw new KeyNotFoundException($"Location with id {transaction.LocationToId} was not found.");

        await _transactionRepository.UpdateTransactionAsync(transaction);
    }
}