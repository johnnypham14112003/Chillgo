namespace Chillgo.Repository.Models;

public partial class ChillCoinTask
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int RewardCoin { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CustomerChillCoinTask> CustomerChillCoinTasks { get; set; } = new List<CustomerChillCoinTask>();
}
