namespace Chillgo.Repository.Models;

public partial class CustomerChillCoinTask
{
    public Guid ChillCoinTaskId { get; set; }

    public Guid AccountId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual ChillCoinTask ChillCoinTask { get; set; } = null!;
}
