namespace Chillgo.Repository.Models;

public partial class SalaryTransaction
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public DateTime PaidAt { get; set; }

    public decimal BaseSalary { get; set; }

    public decimal Bonus { get; set; }

    public string PayMethod { get; set; } = null!;

    public decimal TotalPaid { get; set; }

    public string Content { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;
}
