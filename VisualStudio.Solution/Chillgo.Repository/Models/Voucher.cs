namespace Chillgo.Repository.Models;

public partial class Voucher
{
    public Guid Id { get; set; }

    public string? ExchangeCode { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime AvailableDate { get; set; }

    public DateTime ExpiredDate { get; set; }

    public decimal MinimumTransaction { get; set; }

    public byte DiscountPercent { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CustomerVoucher> CustomerVouchers { get; set; } = new List<CustomerVoucher>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
