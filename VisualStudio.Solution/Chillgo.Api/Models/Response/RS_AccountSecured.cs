using System.ComponentModel.DataAnnotations;

namespace Chillgo.Api.Models.Response
{
    public class RS_AccountSecured
    {
        public Guid Id { get; set; }
        [Required] public required string FirebaseUid { get; set; }
        [Required] public required string Email { get; set; }
        [Required] public required string FullName { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Cccd { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public DateTime JoinedDate { get; set; }

        public DateTime LastUpdated { get; set; }

        public int ChillCoin { get; set; }

        public string? Language { get; set; }

        [Required] public required string Role { get; set; }

        [Required] public required string Status { get; set; }
    }
}
