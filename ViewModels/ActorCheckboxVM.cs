namespace CinemaTickets.ViewModels;

public class ActorCheckboxVM
{
    public int    Id         { get; set; }
    public string Name       { get; set; } = string.Empty;
    public string? Img       { get; set; }
    public bool   IsSelected { get; set; }
}
