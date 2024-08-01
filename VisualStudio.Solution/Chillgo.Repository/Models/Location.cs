namespace Chillgo.Repository.Models;

public partial class Location
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? Coordinates { get; set; }

    public decimal TicketPrice { get; set; }

    public decimal TotalRating { get; set; }

    public bool IsMarketingPaid { get; set; }

    public Guid? PartnerId { get; set; }

    public DateTime LastUpdated { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Account? Partner { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
