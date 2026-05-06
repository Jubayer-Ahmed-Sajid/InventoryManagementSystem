using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSystem.Models.Location;
public enum LocationType
{
    supplier=1,
    customer=2,
    warehouse=3,
    disposal=4
}

public class Location
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public LocationType LocationType { get; set; }
    public string Address { get; set; } = string.Empty;

}