namespace UGHApi.Shared;

public class QueryParameters
{
    public string SearchTerm { get; set; } = null;
    public string SortBy { get; set; } = null;
    public bool IsSortDescending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
