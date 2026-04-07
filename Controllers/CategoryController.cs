using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Models.Category;


namespace InventoryManagementSystem.Controllers;

public class CategoryController : Controller
{
    private readonly CategoryServices _categoryServices;
    private readonly ProductServices _productServices;

    public CategoryController(CategoryServices categoryServices, ProductServices productServices)
    {
        _categoryServices = categoryServices;
        _productServices = productServices;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryServices.GetAllCategoriesAsync();
        return View("Index", categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("Create");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await _categoryServices.AddCategoryAsync(category);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Category category;
        try
        {
            category = await _categoryServices.GetCategoryByIdAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return View("Edit", category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        var existingProductsWithCategory = await _productServices.GetProductByCategoryIdAsync(category.Id);
        
        await _categoryServices.UpdateCategoryAsync(category);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        Category category;
        try
        {
            category = await _categoryServices.GetCategoryByIdAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return View("Details", category);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Category category;
        try
        {
            category = await _categoryServices.GetCategoryByIdAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return View("Delete", category);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryServices.DeleteCategoryAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error()
    {
        return View();
    }

}
