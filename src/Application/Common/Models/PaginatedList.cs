namespace Porcupine.Application.Common.Models;

public record PaginatedList<T>(IReadOnlyCollection<T> Items, int Count, int PageNumber, int PageSize)
{
    public int TotalPages { get; } = (int)Math.Ceiling(Count / (double)PageSize);

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
