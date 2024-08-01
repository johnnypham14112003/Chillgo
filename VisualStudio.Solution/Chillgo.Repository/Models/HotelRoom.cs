namespace Chillgo.Repository.Models;

public partial class HotelRoom
{
    public Guid Id { get; set; }

    public Guid HotelId { get; set; }

    public string? RoomNo { get; set; }

    public string? Floor { get; set; }

    public string Type { get; set; } = null!;

    public string? Utilities { get; set; }

    public decimal PricePerNight { get; set; }

    public byte Capacity { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Hotel Hotel { get; set; } = null!;
}
