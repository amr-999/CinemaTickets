using CinemaTickets.Helpers;
using CinemaTickets.Models;
using CinemaTickets.Services;
using CinemaTickets.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaTickets.Areas.Admin.Controllers;

[Area("Admin")]
public class HallController : Controller
{
    private readonly IHallService   _halls;
    private readonly ICinemaService _cins;
    public HallController(IHallService h, ICinemaService c) { _halls = h; _cins = c; }

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 10;
        var all = await _halls.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(search))
            all = all.Where(h => h.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                  (h.Cinema?.Name ?? "").Contains(search, StringComparison.OrdinalIgnoreCase));
        var paged = PaginatedList<Hall>.Create(all, page, pageSize);
        ViewBag.Search = search; ViewBag.Page = page;
        ViewBag.TotalPages = paged.TotalPages; ViewBag.TotalCount = paged.TotalCount;
        return View(paged);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new HallVM { CinemaList = new SelectList(await _cins.GetAllAsync(), "Id", "Name") };
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HallVM vm)
    {
        ModelState.Remove("CinemaList");
        if (!ModelState.IsValid)
        {
            vm.CinemaList = new SelectList(await _cins.GetAllAsync(), "Id", "Name", vm.CinemaId);
            return View(vm);
        }
        var h = new Hall { Name = vm.Name, TotalSeats = vm.TotalSeats,
                           Description = vm.Description, CinemaId = vm.CinemaId };
        await _halls.AddAsync(h);
        TempData["Success"] = $"Hall \"{h.Name}\" created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var h = await _halls.GetByIdAsync(id);
        if (h is null) return NotFound();
        var vm = new HallVM { Id = h.Id, Name = h.Name, TotalSeats = h.TotalSeats,
                              Description = h.Description, CinemaId = h.CinemaId,
                              CinemaList  = new SelectList(await _cins.GetAllAsync(), "Id", "Name", h.CinemaId) };
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, HallVM vm)
    {
        if (id != vm.Id) return BadRequest();
        ModelState.Remove("CinemaList");
        if (!ModelState.IsValid)
        {
            vm.CinemaList = new SelectList(await _cins.GetAllAsync(), "Id", "Name", vm.CinemaId);
            return View(vm);
        }
        var h = await _halls.GetByIdAsync(id); if (h is null) return NotFound();
        h.Name = vm.Name; h.TotalSeats = vm.TotalSeats;
        h.Description = vm.Description; h.CinemaId = vm.CinemaId;
        await _halls.UpdateAsync(h);
        TempData["Success"] = $"Hall \"{h.Name}\" updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var h = await _halls.GetByIdAsync(id); if (h is null) return NotFound();
            await _halls.DeleteAsync(id);
            TempData["Success"] = $"Hall \"{h.Name}\" deleted.";
        }
        catch { TempData["Error"] = "Cannot delete – movies are linked to this hall."; }
        return RedirectToAction(nameof(Index));
    }
}
