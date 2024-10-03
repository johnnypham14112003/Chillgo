namespace Chillgo.BusinessService.BusinessModels
{
    public class BM_AccountQuery
    {
        public string? KeyWord { get; set; }
        public string? Gender { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool NameDescendingOrder { get; set; } = true;
    }
}
