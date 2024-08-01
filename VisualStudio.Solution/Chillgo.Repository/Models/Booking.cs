namespace Chillgo.Repository.Models;

public partial class Booking
{
    public Guid Id { get; set; }

    public DateTime BookedDate { get; set; }

    public string PayMethod { get; set; } = null!;

    public int ChillCoinApplied { get; set; }

    public decimal TotalPrice { get; set; }

    public string Note { get; set; } = null!;

    public string Status { get; set; } = null!;

    public Guid AccountId { get; set; }

    public Guid? VoucherId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Voucher? Voucher { get; set; }
}
