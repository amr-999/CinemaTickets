using CinemaTickets.Models.Enums;

namespace CinemaTickets.Extensions;

public static class MovieStatusExtensions
{
    public static string ToDisplay(this MovieStatus s) => s switch
    {
        MovieStatus.ComingSoon => "Coming Soon",
        MovieStatus.NowShowing => "Now Showing",
        MovieStatus.Ended      => "Ended",
        _                      => s.ToString()
    };

    public static string ToBadge(this MovieStatus s) => s switch
    {
        MovieStatus.ComingSoon => "badge bg-warning text-dark",
        MovieStatus.NowShowing => "badge bg-success",
        MovieStatus.Ended      => "badge bg-secondary",
        _                      => "badge bg-primary"
    };
}
