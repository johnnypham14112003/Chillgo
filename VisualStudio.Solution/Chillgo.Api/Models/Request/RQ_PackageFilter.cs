namespace Chillgo.Api.Models.Request
{
    public class RQ_PackageFilter
    {
        public string? KeyWord { get; set; }

        public decimal? Price { get; set; }

        public short? Duration { get; set; }

        public string? Status { get; set; } = "Đang Bán";
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool NameDescendingOrder { get; set; } = true;
    }
}
