namespace Chillgo.BusinessService.BusinessModels
{
    public class BM_PagingResults<T> where T : class
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        public List<T> DataList { get; set; } = [];
    }
}
