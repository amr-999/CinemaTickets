using CinemaTickets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaTickets.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly IMovieService    _movies;
    private readonly ICategoryService _cats;
    private readonly ICinemaService   _cins;

    public HomeController(IMovieService m, ICategoryService c, ICinemaService ci)
    { _movies = m; _cats = c; _cins = ci; }

    public async Task<IActionResult> Index(
        string? search, int? categoryId, int? cinemaId, string? status, int page = 1)
    {
        const int pageSize = 6;
        var (items, total) = await _movies.GetFilteredAsync(
            search, categoryId, cinemaId, status, page, pageSize, availableOnly: true);

        ViewBag.Search     = search;
        ViewBag.CategoryId = categoryId;
        ViewBag.CinemaId   = cinemaId;
        ViewBag.Status     = status;
        ViewBag.Page       = page;
        ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
        ViewBag.TotalCount = total;
        ViewBag.PageSize   = pageSize;
        ViewBag.Categories = new SelectList(await _cats.GetAllAsync(), "Id", "Name", categoryId);
        ViewBag.Cinemas    = new SelectList(await _cins.GetAllAsync(), "Id", "Name", cinemaId);
        return View(items);
    }

    public async Task<IActionResult> Details(int id)
    {
        var m = await _movies.GetWithDetailsAsync(id);
        return m is null ? NotFound() : View(m);
    }
}
