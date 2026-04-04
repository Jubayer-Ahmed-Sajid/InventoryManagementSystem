using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSystem.Models.Category;
public class Category
{
    [Required]
    public int Id { get; set; }
    [Required, StringLength(100), RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, and spaces.")]
    public string Name { get; set; } = string.Empty;
   
}