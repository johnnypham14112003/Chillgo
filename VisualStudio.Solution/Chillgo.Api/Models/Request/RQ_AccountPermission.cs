using System.ComponentModel.DataAnnotations;

namespace Chillgo.Api.Models.Request
{
    public class RQ_AccountPermission
    {
        public Guid targetAccountId { get; set; } = Guid.Empty;
        [Required][EmailAddress] public required string Email { get; set; }
        [Required] public required string Password { get; set; }
        public string? NewPassword { get; set; }
        [Required] public required string Role { get; set; }
    }
}
