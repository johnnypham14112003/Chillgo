namespace Chillgo.Repository.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
