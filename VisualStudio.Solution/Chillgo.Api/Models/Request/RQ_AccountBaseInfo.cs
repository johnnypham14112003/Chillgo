namespace Chillgo.Api.Models.Request
{
    public class RQ_AccountBaseInfo
    {
        public Guid? Id { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Cccd { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public DateTime? LastUpdated { get; set; }

        public string? Expertise { get; set; }

        public string? Language { get; set; }

        public string? CompanyName { get; set; }
    }
}
