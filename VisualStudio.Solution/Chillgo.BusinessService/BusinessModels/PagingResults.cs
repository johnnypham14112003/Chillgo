namespace Chillgo.BusinessService.BusinessModels
{
    public class PagingResults<T> where T : class
    {
        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public List<T> Datas { get; set; } = [];

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
    }
}
