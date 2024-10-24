using Microsoft.AspNetCore.Http;

namespace Chillgo.BusinessService.BusinessModels
{
    public class BM_Image
    {
        public string? UrlPath { get; set; }

        public bool IsAvatar { get; set; }

        public byte Type { get; set; }

        public string Status { get; set; } = null!;

        public Guid? AccountId { get; set; }

        public Guid? CertificateId { get; set; }

        public Guid? LocationId { get; set; }

        public Guid? HotelId { get; set; }

        public Guid? TransportId { get; set; }

        public Guid? BlogId { get; set; }

        public Guid? VoucherId { get; set; }
    }
}
