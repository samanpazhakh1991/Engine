namespace Framework.Contracts.Common
{
    public class PageInfo
    {
        public int? LastId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int? CurrentPage { get; set; }

    }
}
