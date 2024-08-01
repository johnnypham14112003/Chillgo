namespace Chillgo.Repository.Models;

public partial class Transport
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public byte LuggageSlot { get; set; }

    public byte SitSlot { get; set; }

    public string Provider { get; set; } = null!;

    public decimal TotalRating { get; set; }

    public decimal PricePerDay { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
