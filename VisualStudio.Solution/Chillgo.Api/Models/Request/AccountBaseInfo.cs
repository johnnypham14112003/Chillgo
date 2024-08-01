using System.ComponentModel.DataAnnotations;

namespace Chillgo.Api.Models.Request
{
    public class AccountBaseInfo
    {
        [Required][EmailAddress] public required string Email { get; set; }
        [Required] public required string FullName { get; set; }

        [Required] public required string Password { get; set; }
    }
}
