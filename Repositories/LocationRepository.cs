using InventoryManagementSystem.Models.Location;
namespace InventoryManagementSystem.Repositories;
using System.Collections.Generic;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetAllLocationsAsync();
    Task<Location> GetLocationByIdAsync(int id);
    Task<IEnumerable<Location>> GetLocationsByLocationTypeAsync(LocationType locationType);
    Task AddLocationAsync(Location location);
    
}
public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LocationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync()
    {
        return await _dbContext.Locations
            .Select(location => new Location
            {
                Id = location.Id,
                Name = location.Name ?? string.Empty,
                LocationType = location.LocationType,
                Address = location.Address ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<Location> GetLocationByIdAsync(int id)
    {
        return await _dbContext.Locations
            .Where(location => location.Id == id)
            .Select(location => new Location
            {
                Id = location.Id,
                Name = location.Name ?? string.Empty,
                LocationType = location.LocationType,
                Address = location.Address ?? string.Empty
            })
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"Location with id {id} was not found.");
    }

    public async Task<IEnumerable<Location>> GetLocationsByLocationTypeAsync(LocationType locationType)
    {
        return await _dbContext.Locations
            .Where(l => l.LocationType == locationType)
            .Select(location => new Location
            {
                Id = location.Id,
                Name = location.Name ?? string.Empty,
                LocationType = location.LocationType,
                Address = location.Address ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task AddLocationAsync(Location location)
    {
        _dbContext.Locations.Add(location);
        await _dbContext.SaveChangesAsync();
    }
}
