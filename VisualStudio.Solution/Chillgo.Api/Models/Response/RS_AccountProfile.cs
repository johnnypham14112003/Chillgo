namespace Chillgo.Api.Models.Response
{
    public class RS_AccountProfile
    {
        public Guid Id { get; set; }
        public string FirebaseUid { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Cccd { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public DateTime JoinedDate { get; set; }

        public DateTime LastUpdated { get; set; }

        public int ChillCoin { get; set; }

        public string? Language { get; set; }

        public string Role { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
