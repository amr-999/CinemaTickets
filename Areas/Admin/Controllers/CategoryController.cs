using CinemaTickets.Helpers;
using CinemaTickets.Models;
using CinemaTickets.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTickets.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _svc;
    public CategoryController(ICategoryService svc) => _svc = svc;

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 10;
        var all = await _svc.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(search))
            all = all.Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        var paged = PaginatedList<Category>.Create(all, page, pageSize);
        ViewBag.Search = search; ViewBag.Page = page;
        ViewBag.TotalPages = paged.TotalPages; ViewBag.TotalCount = paged.TotalCount;
        return View(paged);
    }

    public IActionResult Create() => View(new Category());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model)
    {
        if (!ModelState.IsValid) return View(model);
        await _svc.AddAsync(model);
        TempData["Success"] = $"Category \"{model.Name}\" created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var c = await _svc.GetByIdAsync(id);
        return c is null ? NotFound() : View(c);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        await _svc.UpdateAsync(model);
        TempData["Success"] = $"Category \"{model.Name}\" updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var c = await _svc.GetByIdAsync(id);
            if (c is null) return NotFound();
            await _svc.DeleteAsync(id);
            TempData["Success"] = $"Category \"{c.Name}\" deleted.";
        }
        catch { TempData["Error"] = "Cannot delete – movies are linked to this category."; }
        return RedirectToAction(nameof(Index));
    }
}
