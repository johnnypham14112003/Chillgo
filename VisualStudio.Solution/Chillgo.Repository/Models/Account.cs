namespace Chillgo.Repository.Models;

public partial class Account
{
    public Guid Id { get; set; }

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

    public string? Expertise { get; set; }

    public string? Language { get; set; }

    public decimal Rating { get; set; }

    public string? CompanyName { get; set; }

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Comment> CommentPeople { get; set; } = new List<Comment>();

    public virtual ICollection<Comment> CommentSenders { get; set; } = new List<Comment>();

    public virtual ICollection<Conversation> ConversationFirstAccounts { get; set; } = new List<Conversation>();

    public virtual ICollection<Conversation> ConversationSecondAccounts { get; set; } = new List<Conversation>();

    public virtual ICollection<CustomerChillCoinTask> CustomerChillCoinTasks { get; set; } = new List<CustomerChillCoinTask>();

    public virtual ICollection<CustomerVoucher> CustomerVouchers { get; set; } = new List<CustomerVoucher>();

    public virtual ICollection<FavoritedPerson> FavoritedPersonAccounts { get; set; } = new List<FavoritedPerson>();

    public virtual ICollection<FavoritedPerson> FavoritedPersonPeople { get; set; } = new List<FavoritedPerson>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Location> LocationsNavigation { get; set; } = new List<Location>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<PackageTransaction> PackageTransactions { get; set; } = new List<PackageTransaction>();

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();

    public virtual ICollection<SalaryTransaction> SalaryTransactions { get; set; } = new List<SalaryTransaction>();

    public virtual ICollection<VerificationRequest> VerificationRequestSenders { get; set; } = new List<VerificationRequest>();

    public virtual ICollection<VerificationRequest> VerificationRequestStaffVerifies { get; set; } = new List<VerificationRequest>();

    public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();

    public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<Transport> Transports { get; set; } = new List<Transport>();
}
