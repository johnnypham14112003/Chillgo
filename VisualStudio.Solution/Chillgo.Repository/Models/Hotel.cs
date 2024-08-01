namespace Chillgo.Repository.Models;

public partial class Hotel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Utilities { get; set; }

    public string? Address { get; set; }

    public byte TotalFloor { get; set; }

    public string Hotline { get; set; } = null!;

    public decimal TotalRating { get; set; }

    public bool IsMarketingPaid { get; set; }

    public string? PriceRange { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual ICollection<HotelRoom> HotelRooms { get; set; } = new List<HotelRoom>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
