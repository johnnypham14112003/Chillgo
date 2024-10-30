namespace Chillgo.Api.Models.Request
{
    public class RQ_PackageFilter
    {
        public string? KeyWord { get; set; }

        public decimal? Price { get; set; }

        public short? Duration { get; set; }

        public string? Status { get; set; } = "Đang Bán";
    }
}
