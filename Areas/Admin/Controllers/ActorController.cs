using CinemaTickets.Helpers;
using CinemaTickets.Models;
using CinemaTickets.Services;
using CinemaTickets.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTickets.Areas.Admin.Controllers;

[Area("Admin")]
public class ActorController : Controller
{
    private readonly IActorService _svc;
    private readonly IImageService _img;
    public ActorController(IActorService svc, IImageService img) { _svc = svc; _img = img; }

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 12;
        var all = await _svc.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(search))
            all = all.Where(a => a.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        var paged = PaginatedList<Actor>.Create(all, page, pageSize);
        ViewBag.Search = search; ViewBag.Page = page;
        ViewBag.TotalPages = paged.TotalPages; ViewBag.TotalCount = paged.TotalCount;
        return View(paged);
    }

    public IActionResult Create() => View(new ActorVM());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ActorVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var a = new Actor { Name = vm.Name, Bio = vm.Bio,
                            Img = await _img.SaveImageAsync(vm.ImgFile, "actors") };
        await _svc.AddAsync(a);
        TempData["Success"] = $"Actor \"{a.Name}\" created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var a = await _svc.GetByIdAsync(id);
        if (a is null) return NotFound();
        return View(new ActorVM { Id = a.Id, Name = a.Name, Bio = a.Bio, Img = a.Img });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ActorVM vm)
    {
        if (id != vm.Id) return BadRequest();
        if (!ModelState.IsValid) return View(vm);
        var a = await _svc.GetByIdAsync(id); if (a is null) return NotFound();
        a.Name = vm.Name; a.Bio = vm.Bio;
        if (vm.ImgFile != null) { _img.DeleteImage(a.Img); a.Img = await _img.SaveImageAsync(vm.ImgFile, "actors"); }
        await _svc.UpdateAsync(a);
        TempData["Success"] = $"Actor \"{a.Name}\" updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var a = await _svc.GetByIdAsync(id); if (a is null) return NotFound();
            _img.DeleteImage(a.Img); await _svc.DeleteAsync(id);
            TempData["Success"] = $"Actor \"{a.Name}\" deleted.";
        }
        catch { TempData["Error"] = "Cannot delete this actor."; }
        return RedirectToAction(nameof(Index));
    }
}
