using UGHApi.Shared;

namespace UGHApi.Repositories
{
    public class UserQueryParameters : QueryParameters
    {
        public string SortDirection { get; set; } = "asc";
    }
}
