using CinemaTickets.Services;
using CinemaTickets.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTickets.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly IMovieService    _movies;
    private readonly ICategoryService _cats;
    private readonly ICinemaService   _cins;
    private readonly IActorService    _actors;

    public HomeController(IMovieService m, ICategoryService c,
                          ICinemaService ci, IActorService a)
    { _movies = m; _cats = c; _cins = ci; _actors = a; }

    public async Task<IActionResult> Index()
    {
        var (total, nowShowing, comingSoon, ended) = await _movies.GetStatsAsync();
        var all = await _movies.GetAllAsync();

        var vm = new DashboardVM
        {
            TotalMovies     = total,
            TotalCategories = (await _cats.GetAllAsync()).Count(),
            TotalCinemas    = (await _cins.GetAllAsync()).Count(),
            TotalActors     = (await _actors.GetAllAsync()).Count(),
            NowShowing      = nowShowing,
            ComingSoon      = comingSoon,
            Ended           = ended,
            RecentMovies    = all.OrderByDescending(m => m.CreatedAt).Take(6).ToList()
        };
        return View(vm);
    }
}
