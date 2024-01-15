namespace Framework.Contracts.Common
{
    public class PageResult<T>
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        
        public List<T> Result { get; set; }

        public PageResult()
        {
            Result = new List<T>();
        }
    }
}
