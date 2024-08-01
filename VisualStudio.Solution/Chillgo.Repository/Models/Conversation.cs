namespace Chillgo.Repository.Models;

public partial class Conversation
{
    public Guid Id { get; set; }

    public bool IsHuman { get; set; }

    public string? FirstName { get; set; }

    public Guid? FirstAccountId { get; set; }

    public string? SecondName { get; set; }

    public Guid? SecondAccountId { get; set; }

    public Guid? AibotId { get; set; }

    public DateTime LastUpdated { get; set; }

    public string Status { get; set; } = null!;

    public virtual BotAi? Aibot { get; set; }

    public virtual Account? FirstAccount { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Account? SecondAccount { get; set; }
}
