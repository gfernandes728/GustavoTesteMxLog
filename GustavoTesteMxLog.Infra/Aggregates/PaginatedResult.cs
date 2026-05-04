namespace GustavoTesteMxLog.Infra.Aggregates;

public class PaginatedResult<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
