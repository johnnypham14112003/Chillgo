namespace Chillgo.BusinessService.BusinessModels
{
    public class BM_PagingResults<T> where T : class
    {
        public int TotalCount { get; set; }
        public List<T> DataList { get; set; } = [];
    }
}
