namespace Chillgo.Repository.Models;

public partial class PackageTransaction
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid PackageId { get; set; }

    public int ChillCoinApplied { get; set; }

    public DateTime PaidAt { get; set; }

    public string PayMethod { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? VoucherCodeList { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual Package Package { get; set; } = null!;
}
