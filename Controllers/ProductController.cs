using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Models.Product;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace InventoryManagementSystem.Controllers;

public class ProductController : Controller
{
    private readonly ProductServices _productServices;
    private readonly CategoryServices _categoryServices;
    private readonly SupplierServices _supplierServices;

    public ProductController(ProductServices productServices, CategoryServices categoryServices, SupplierServices supplierServices)
    {
        _productServices = productServices;
        _categoryServices = categoryServices;
        _supplierServices = supplierServices;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productServices.GetAllProductsAsync();
        return View(products);
    }
    [HttpPost]
    public async Task<IActionResult> Index(string searchString)
    {
            ViewData["CurrentFilter"] = searchString;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return RedirectToAction("Index");
            }
    
            var products = await _productServices.GetProductByNameAsync(searchString);
            return View("Index", products);

        
    }

    public async Task<IActionResult> Create()
    {
        await LoadDropDownsAsync();
        return View("Create");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropDownsAsync();
            return View(product);
        }

        await _productServices.AddProductAsync(product);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Product product;
        try
        {
            product = await _productServices.GetProductByIdAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        await LoadDropDownsAsync();
        return View("Edit", product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropDownsAsync();
            return View(product);
        }

        await _productServices.UpdateProductAsync(product);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        Product product;
        try
        {
            product = await _productServices.GetProductByIdAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return View("Details", product);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Product product;
        try
        {
            product = await _productServices.GetProductByIdAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return View("Delete", product);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productServices.DeleteProductAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error()
    {
        return View();
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

        var suppliers = (await _supplierServices.GetAllSuppliersAsync())
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            })
            .ToList();

        ViewBag.Categories = categories;
        ViewBag.Suppliers = suppliers;
    }

}
