using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    
    public IActionResult Error()
    {
        return View();
    }
}
