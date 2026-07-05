using CinemaTickets.Models;

namespace CinemaTickets.ViewModels;

public class DashboardVM
{
    public int TotalMovies     { get; set; }
    public int TotalCategories { get; set; }
    public int TotalCinemas    { get; set; }
    public int TotalActors     { get; set; }
    public int NowShowing      { get; set; }
    public int ComingSoon      { get; set; }
    public int Ended           { get; set; }
    public List<Movie> RecentMovies { get; set; } = [];
}
