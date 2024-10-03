using System.ComponentModel.DataAnnotations;

namespace Chillgo.Api.Models.Request
{
    public class RQ_AccountAuth
    {
        public string? FullName { get; set; }
        [Required][EmailAddress] public required string Email { get; set; }
        [Required] public required string Password { get; set; }
    }
}
