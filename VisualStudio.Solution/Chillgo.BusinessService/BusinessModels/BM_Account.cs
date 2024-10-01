using Chillgo.Repository.Models;

namespace Chillgo.BusinessService.BusinessModels
{
    public class BM_Account
    {
        public Guid Id { get; set; } = Guid.Empty;

        public string FirebaseUid { get; set; } = "";

        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Address { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public string Cccd { get; set; } = "";

        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;

        public string Gender { get; set; } = "";

        public DateTime JoinedDate { get; set; } = DateTime.MinValue;

        public DateTime LastUpdated { get; set; } = DateTime.MinValue;

        public int ChillCoin { get; set; } = 0;

        public string FcmToken { get; set; } = "";

        public string GoogleId { get; set; } = "";

        public string FacebookId { get; set; } = "";

        public string Expertise { get; set; } = "";

        public string Language { get; set; } = "";

        public decimal Rating { get; set; } = 0;

        public string CompanyName { get; set; } = "";

        public string Role { get; set; } = "";

        public string Status { get; set; } = "";
    }
}
