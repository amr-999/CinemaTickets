using CinemaTickets.Models;
using CinemaTickets.Models.Enums;
using CinemaTickets.Services;
using CinemaTickets.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaTickets.Areas.Admin.Controllers;

[Area("Admin")]
public class MovieController : Controller
{
    private readonly IMovieService    _movies;
    private readonly ICategoryService _cats;
    private readonly ICinemaService   _cins;
    private readonly IHallService     _halls;
    private readonly IActorService    _actors;
    private readonly IImageService    _img;

    public MovieController(IMovieService m, ICategoryService c, ICinemaService ci,
                           IHallService h, IActorService a, IImageService img)
    { _movies = m; _cats = c; _cins = ci; _halls = h; _actors = a; _img = img; }

    // INDEX + FILTER + PAGINATION
    public async Task<IActionResult> Index(
        string? search, int? categoryId, int? cinemaId, string? status, int page = 1)
    {
        const int pageSize = 10;
        var (items, total) = await _movies.GetFilteredAsync(
            search, categoryId, cinemaId, status, page, pageSize);

        ViewBag.Search      = search;
        ViewBag.CategoryId  = categoryId;
        ViewBag.CinemaId    = cinemaId;
        ViewBag.Status      = status;
        ViewBag.Page        = page;
        ViewBag.TotalPages  = (int)Math.Ceiling(total / (double)pageSize);
        ViewBag.TotalCount  = total;
        ViewBag.PageSize    = pageSize;
        ViewBag.Categories  = new SelectList(await _cats.GetAllAsync(), "Id", "Name", categoryId);
        ViewBag.Cinemas     = new SelectList(await _cins.GetAllAsync(), "Id", "Name", cinemaId);
        return View(items);
    }

    public async Task<IActionResult> Details(int id)
    {
        var m = await _movies.GetWithDetailsAsync(id);
        return m is null ? NotFound() : View(m);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new MovieVM();
        await Populate(vm);
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovieVM vm)
    {
        Clean();
        if (!ModelState.IsValid) { await Populate(vm); return View(vm); }

        var movie = Map(vm);
        movie.MainImg = await _img.SaveImageAsync(vm.MainImgFile, "movies");
        if (vm.SubImgFiles?.Count > 0)
            foreach (var p in await _img.SaveImagesAsync(vm.SubImgFiles, "movies"))
                movie.SubImages.Add(new MovieSubImage { Img = p });

        await _movies.AddAsync(movie, vm.SelectedActorIds);
        TempData["Success"] = $"Movie \"{vm.Name}\" created!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var movie = await _movies.GetWithDetailsAsync(id);
        if (movie is null) return NotFound();
        var selIds = movie.MovieActors.Select(ma => ma.ActorId).ToHashSet();
        var vm = new MovieVM
        {
            Id = movie.Id, Name = movie.Name, Description = movie.Description,
            Price = movie.Price, Status = movie.Status, DateTime = movie.DateTime,
            AvailableSeats = movie.AvailableSeats,
            MainImg = movie.MainImg, CategoryId = movie.CategoryId,
            CinemaId = movie.CinemaId, HallId = movie.HallId,
            ExistingSubImages = movie.SubImages.ToList(),
            SelectedActorIds  = selIds.ToList()
        };
        await Populate(vm, selIds);
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MovieVM vm)
    {
        if (id != vm.Id) return BadRequest();
        Clean();
        if (!ModelState.IsValid)
        {
            vm.ExistingSubImages = (await _movies.GetWithDetailsAsync(id))?.SubImages.ToList() ?? [];
            await Populate(vm, vm.SelectedActorIds.ToHashSet());
            return View(vm);
        }
        var movie = await _movies.GetByIdAsync(id);
        if (movie is null) return NotFound();
        movie.Name = vm.Name; movie.Description = vm.Description;
        movie.Price = vm.Price; movie.Status = vm.Status;
        movie.DateTime = vm.DateTime; movie.AvailableSeats = vm.AvailableSeats;
        movie.CategoryId = vm.CategoryId; movie.CinemaId = vm.CinemaId;
        movie.HallId = vm.HallId;
        if (vm.MainImgFile != null)
            { _img.DeleteImage(movie.MainImg); movie.MainImg = await _img.SaveImageAsync(vm.MainImgFile, "movies"); }
        if (vm.SubImgFiles?.Count > 0)
            foreach (var p in await _img.SaveImagesAsync(vm.SubImgFiles, "movies"))
                movie.SubImages.Add(new MovieSubImage { Img = p, MovieId = movie.Id });
        await _movies.UpdateAsync(movie, vm.SelectedActorIds, vm.SubImageIdsToRemove);
        TempData["Success"] = $"Movie \"{movie.Name}\" updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _movies.GetWithDetailsAsync(id);
        if (movie is null) return NotFound();
        _img.DeleteImage(movie.MainImg);
        foreach (var si in movie.SubImages) _img.DeleteImage(si.Img);
        await _movies.DeleteAsync(id);
        TempData["Success"] = $"Movie \"{movie.Name}\" deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task Populate(MovieVM vm, HashSet<int>? selIds = null)
    {
        selIds ??= vm.SelectedActorIds.ToHashSet();
        vm.CategoryList = new SelectList(await _cats.GetAllAsync(),  "Id", "Name", vm.CategoryId);
        vm.CinemaList   = new SelectList(await _cins.GetAllAsync(),  "Id", "Name", vm.CinemaId);
        vm.HallList     = new SelectList(await _halls.GetAllAsync(), "Id", "Name", vm.HallId);
        vm.ActorList    = (await _actors.GetAllAsync()).Select(a => new ActorCheckboxVM
        {
            Id = a.Id, Name = a.Name, Img = a.Img, IsSelected = selIds.Contains(a.Id)
        }).ToList();
    }

    private static Movie Map(MovieVM vm) => new()
    {
        Name = vm.Name, Description = vm.Description, Price = vm.Price,
        Status = vm.Status, DateTime = vm.DateTime, AvailableSeats = vm.AvailableSeats,
        CategoryId = vm.CategoryId, CinemaId = vm.CinemaId, HallId = vm.HallId,
        CreatedAt = DateTime.UtcNow
    };

    private void Clean()
    {
        foreach (var k in new[] { "CategoryList","CinemaList","HallList",
                                   "ActorList","ExistingSubImages","SubImgFiles","MainImgFile" })
            ModelState.Remove(k);
    }
}
