using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Models.Supplier;

namespace InventoryManagementSystem.Controllers;

public class SupplierController : Controller
{
	private readonly SupplierServices _supplierServices;

	public SupplierController(SupplierServices supplierServices)
	{
		_supplierServices = supplierServices;
	}

	public async Task<IActionResult> Index()
	{
		var suppliers = await _supplierServices.GetAllSuppliersAsync();
		return View("Index", suppliers);
	}

	[HttpGet]
	public IActionResult Create()
	{
		return View("Create");
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Supplier supplier)
	{
		

		await _supplierServices.AddSupplierAsync(supplier);
		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		Supplier supplier;
		try
		{
			supplier = await _supplierServices.GetSupplierByIdAsync(id);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}

		return View("Edit", supplier);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(Supplier supplier)
	{
	

		await _supplierServices.UpdateSupplierAsync(supplier);
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Details(int id)
	{
		Supplier supplier;
		try
		{
			supplier = await _supplierServices.GetSupplierByIdAsync(id);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}

		return View("Details", supplier);
	}

	[HttpGet]
	public async Task<IActionResult> Delete(int id)
	{
		Supplier supplier;
		try
		{
			supplier = await _supplierServices.GetSupplierByIdAsync(id);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}

		return View("Delete", supplier);
	}

	[HttpPost]
	[ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _supplierServices.DeleteSupplierAsync(id);
		return RedirectToAction(nameof(Index));
	}
}
