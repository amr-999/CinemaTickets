using Microsoft.AspNetCore.Mvc;

namespace CinemaTickets.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() =>
        RedirectToAction("Index", "Home", new { area = "Customer" });
}
