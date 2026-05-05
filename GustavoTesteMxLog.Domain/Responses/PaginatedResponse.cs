namespace GustavoTesteMxLog.Domain.Responses;

public class PaginatedResponse<T>
    (
        List<T>? data,
        int total,
        int page,
        int pageSize
    )
{
    public List<T> Data { get; private set; } = data ?? [];
    public int Total { get; private set; } = total;
    public int Page { get; private set; } = page;
    public int PageSize { get; private set; } = pageSize;
    public int TotalPages { get; private set; } = (int)Math.Ceiling(total / (double)pageSize);
}
