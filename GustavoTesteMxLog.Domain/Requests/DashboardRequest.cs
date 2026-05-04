namespace GustavoTesteMxLog.Domain.Requests;

public class DashboardRequest
{
    public string? Search { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
