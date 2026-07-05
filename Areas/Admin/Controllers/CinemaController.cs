using CinemaTickets.Helpers;
using CinemaTickets.Models;
using CinemaTickets.Services;
using CinemaTickets.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTickets.Areas.Admin.Controllers;

[Area("Admin")]
public class CinemaController : Controller
{
    private readonly ICinemaService _svc;
    private readonly IImageService  _img;
    public CinemaController(ICinemaService svc, IImageService img) { _svc = svc; _img = img; }

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 9;
        var all = await _svc.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(search))
            all = all.Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                  (c.Location ?? "").Contains(search, StringComparison.OrdinalIgnoreCase));
        var paged = PaginatedList<Cinema>.Create(all, page, pageSize);
        ViewBag.Search = search; ViewBag.Page = page;
        ViewBag.TotalPages = paged.TotalPages; ViewBag.TotalCount = paged.TotalCount;
        return View(paged);
    }

    public IActionResult Create() => View(new CinemaVM());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CinemaVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var c = new Cinema { Name = vm.Name, Location = vm.Location,
                             Img = await _img.SaveImageAsync(vm.ImgFile, "cinemas") };
        await _svc.AddAsync(c);
        TempData["Success"] = $"Cinema \"{c.Name}\" created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var c = await _svc.GetByIdAsync(id);
        if (c is null) return NotFound();
        return View(new CinemaVM { Id = c.Id, Name = c.Name, Location = c.Location, Img = c.Img });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CinemaVM vm)
    {
        if (id != vm.Id) return BadRequest();
        if (!ModelState.IsValid) return View(vm);
        var c = await _svc.GetByIdAsync(id); if (c is null) return NotFound();
        c.Name = vm.Name; c.Location = vm.Location;
        if (vm.ImgFile != null) { _img.DeleteImage(c.Img); c.Img = await _img.SaveImageAsync(vm.ImgFile, "cinemas"); }
        await _svc.UpdateAsync(c);
        TempData["Success"] = $"Cinema \"{c.Name}\" updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var c = await _svc.GetByIdAsync(id); if (c is null) return NotFound();
            _img.DeleteImage(c.Img); await _svc.DeleteAsync(id);
            TempData["Success"] = $"Cinema \"{c.Name}\" deleted.";
        }
        catch { TempData["Error"] = "Cannot delete – movies or halls are linked to this cinema."; }
        return RedirectToAction(nameof(Index));
    }
}
