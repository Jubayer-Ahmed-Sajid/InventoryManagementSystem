namespace InventoryManagementSystem.Models.Product;

using System.ComponentModel.DataAnnotations;

public class Product
{
    [Required]
    public int Id { get; set; }
    [Required, StringLength(100), RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, and spaces.")]
    public string Name { get; set; } = string.Empty;
    [StringLength(250)]

    public string Description { get; set; } = string.Empty;
    [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }
    [Required, Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
}
