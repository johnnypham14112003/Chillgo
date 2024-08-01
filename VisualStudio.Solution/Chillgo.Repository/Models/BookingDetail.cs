namespace Chillgo.Repository.Models;

public partial class BookingDetail
{
    public Guid Id { get; set; }

    public Guid BookingId { get; set; }

    public short NumberOfPeople { get; set; }

    public decimal Subtotal { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public string? RoomType { get; set; }

    public short QuantityRoom { get; set; }

    public Guid? TourGuideId { get; set; }

    public Guid? LocationId { get; set; }

    public Guid? HotelId { get; set; }

    public Guid? TransportId { get; set; }

    public Guid? RoomId { get; set; }

    public Guid? VoucherId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Hotel? Hotel { get; set; }

    public virtual Location? Location { get; set; }

    public virtual HotelRoom? Room { get; set; }

    public virtual Account? TourGuide { get; set; }

    public virtual Transport? Transport { get; set; }

    public virtual Voucher? Voucher { get; set; }
}
