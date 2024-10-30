using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Models.Request
{
    public class RQ_ImageInfo
    {
        public required IFormFile File { get; set; }
        public required Guid FileName { get; set; }
        public bool IsAvatar { get; set; }

        public byte Type { get; set; }

        public required string Status { get; set; }

        public Guid? AccountId { get; set; }
        public Guid? LocationId { get; set; }
    }
}
