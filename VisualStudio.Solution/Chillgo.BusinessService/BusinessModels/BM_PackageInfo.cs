namespace Chillgo.BusinessService.BusinessModels
{
    public class BM_PackageInfo
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public short Duration { get; set; }

        public string Status { get; set; } = null!;
    }
}
