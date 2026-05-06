using System.ComponentModel.DataAnnotations;
namespace InventoryManagementSystem.Models.Transaction;
using InventoryManagementSystem.Models.Product;
using InventoryManagementSystem.Models.Category;

public enum TransactionType
{
    StockIn=1,
    StockOut=2
}   
public class Transaction
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public TransactionType TransactionType { get; set; }
    [Required]
    public int LocationFromId { get; set; } 
    [Required]
    public int LocationToId { get; set; } 
    [Required]
    public DateTime Date { get; set; }= DateTime.Now;
    [Required]
    public string TransactionID { get; set; } = string.Empty;
}