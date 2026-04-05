using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSystem.Models.Supplier;
public class Supplier
{
    [Required]
    public int Id { get; set; }
    [Required, StringLength(200), RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, and spaces.")]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Country { get; set; } = string.Empty;
    [Required,EmailAddress, StringLength(100), RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;
    [Required,StringLength(200)]
    public string Address { get; set; } = string.Empty;
}  
