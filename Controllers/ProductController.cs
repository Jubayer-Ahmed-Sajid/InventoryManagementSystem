using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Models.Category;
using InventoryManagementSystem.Models.Supplier;
using InventoryManagementSystem.Models.Product;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace InventoryManagementSystem.Controllers;

public class ProductController : Controller
{
    // private readonly ILogger<ProductController> _logger;
    private readonly ProductServices _productServices;
    private readonly CategoryServices _categoryServices;
    private readonly SupplierServices _supplierServices;

    public ProductController(ILogger<ProductController> logger, ProductServices productServices, CategoryServices categoryServices, SupplierServices supplierServices)
    {
        // _logger = logger;
        _productServices = productServices;
        _categoryServices = categoryServices;
        _supplierServices = supplierServices;

    }

    public async Task<IActionResult> Index()
    {
        var products = await _productServices.GetAllProductsAsync();
        return View("Index", products);
    }
    public async Task<IActionResult> PopulateDropDowns()
    {
        var AllCategories = await _categoryServices.GetAllCategoriesAsync();
        var AllSuppliers = await _supplierServices.GetAllSuppliersAsync();
        var Categories = AllCategories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();
        var Suppliers = AllSuppliers.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.Name
        }).ToList();
        
        ViewBag.Categories = Categories;
        ViewBag.Suppliers = Suppliers;
        return View("Create");
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropDowns();
        return View("Create");
    }
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
       
            await _productServices.AddProductAsync(product);
            return RedirectToAction("Index");
        
        // await PopulateDropDowns();
        return RedirectToAction("Index", product);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)

    {
        var product = await _productServices.GetProductByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        await PopulateDropDowns();
        return View("Edit", product);
    }

    public IActionResult Error()
    {
        return View();
    }

}
