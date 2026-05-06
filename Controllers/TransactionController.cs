using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Models.Transaction;
using InventoryManagementSystem.Models.Location;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using InventoryManagementSystem.Repositories;
namespace InventoryManagementSystem.Controllers;
using System.Threading.Tasks;
public class TransactionController : Controller
{
     private readonly ProductServices _productServices;
    private readonly CategoryServices _categoryServices;
    private readonly ILocationRepository _locationRepository;
    private readonly TransactionServices _transactionServices;

    public TransactionController(ProductServices productServices, CategoryServices categoryServices, ILocationRepository locationRepository, TransactionServices transactionServices)
    {        _productServices = productServices;
        _categoryServices = categoryServices;
        _locationRepository = locationRepository;
        _transactionServices = transactionServices;
    }
       public async Task<IActionResult> Index()
    {
        var transactions = await _transactionServices.GetAllTransactionsAsync();
        return View("Index", transactions.OrderByDescending(transaction => transaction.Date));
    }
    public async Task<IActionResult> Create()
    {
         await LoadDropDownsAsync();
        return View("Create");
    }

    private async Task LoadDropDownsAsync()
    {
        var categories = (await _categoryServices.GetAllCategoriesAsync())
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToList();
        var products = (await _productServices.GetAllProductsAsync())
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            })
            .ToList();
        
        var suppliers = (await _locationRepository.GetLocationsByLocationTypeAsync(LocationType.supplier))
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Name
            })
            .ToList();
        var customers = (await _locationRepository.GetLocationsByLocationTypeAsync(LocationType.customer))
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Name
            })
            .ToList();
        var warehouses = (await _locationRepository.GetLocationsByLocationTypeAsync(LocationType.warehouse))
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Name
            })
            .ToList();
        var disposals = (await _locationRepository.GetLocationsByLocationTypeAsync(LocationType.disposal))
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Name
            })
            .ToList();

        ViewBag.Categories = categories;
        ViewBag.Products = products;
        ViewBag.Suppliers = suppliers;
        ViewBag.Customers = customers;
        ViewBag.Warehouses = warehouses;
        ViewBag.Disposals = disposals;
    }

   [HttpPost]
public async Task<IActionResult> Create(Transaction transaction)
{
   
    int myWarehouseId = 1; 
    if (transaction.TransactionType == TransactionType.StockIn) 
    {
       
        transaction.LocationToId = myWarehouseId;
    }
    else if (transaction.TransactionType == TransactionType.StockOut)
    {
        
        transaction.LocationFromId = myWarehouseId;
    }
    transaction.Date = DateTime.UtcNow;
    var TransactionCode = $"TXN-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..5].ToUpper()}";
    transaction.TransactionID = TransactionCode;

    await _transactionServices.AddTransactionAsync(transaction);
    return RedirectToAction("Index");
}

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Transaction transaction;
        try
        {
            transaction = await _transactionServices.GetTransactionByIdAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        await LoadDropDownsAsync();
        return View("Edit", transaction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Transaction transaction)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropDownsAsync();
            return View(transaction);
        }

        int myWarehouseId = 1;
        if (transaction.TransactionType == TransactionType.StockIn)
        {
            transaction.LocationToId = myWarehouseId;
        }
        else if (transaction.TransactionType == TransactionType.StockOut)
        {
            transaction.LocationFromId = myWarehouseId;
        }

        await _transactionServices.UpdateTransactionAsync(transaction);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Revert(int id)
    {
        Transaction transaction;
        try
        {
            transaction = await _transactionServices.GetTransactionByIdAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        var revertedTransaction = new Transaction
        {
            ProductId = transaction.ProductId,
            Quantity = transaction.Quantity,
            TransactionType = transaction.TransactionType == TransactionType.StockIn
                ? TransactionType.StockOut
                : TransactionType.StockIn,
            LocationFromId = transaction.LocationToId,
            LocationToId = transaction.LocationFromId,
            Date = DateTime.UtcNow,
            TransactionID = $"REV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..5].ToUpper()}"
        };

        await _transactionServices.AddTransactionAsync(revertedTransaction);
        return RedirectToAction(nameof(Index));
    }

}