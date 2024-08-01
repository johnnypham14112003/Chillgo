namespace Chillgo.Repository.Models;

public partial class CustomerVoucher
{
    public Guid VoucherId { get; set; }

    public Guid AccountId { get; set; }

    public DateTime CollectedDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual Voucher Voucher { get; set; } = null!;
}
