namespace CinemaTickets.Helpers;

public class PaginatedList<T>
{
    public List<T> Items      { get; }
    public int     PageIndex  { get; }
    public int     TotalPages { get; }
    public int     TotalCount { get; }
    public int     PageSize   { get; }

    public bool HasPrevious => PageIndex > 1;
    public bool HasNext     => PageIndex < TotalPages;

    public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items      = items;
        TotalCount = totalCount;
        PageIndex  = pageIndex;
        PageSize   = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
    {
        var list  = source.ToList();
        var total = list.Count;
        var items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, total, pageIndex, pageSize);
    }
}
