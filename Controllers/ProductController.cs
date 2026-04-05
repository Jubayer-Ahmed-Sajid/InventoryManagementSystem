using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly ProductServices _productServices;

    public ProductController(ILogger<ProductController> logger,ProductServices productServices)
    {
        _logger = logger;
        _productServices = productServices;

    }

    public async Task<IActionResult> Index()
    {
        var products = await _productServices.GetAllProductsAsync();
        return View("Index", products);
    }

    
    public IActionResult Error()
    {
        return View();
    }
}
