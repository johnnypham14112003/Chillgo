namespace Chillgo.Repository.Models;

public partial class BotAi
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Capabilities { get; set; }

    public string Provider { get; set; } = null!;

    public string? ClientKey { get; set; }

    public string? Apiendpoint { get; set; }

    public string? Apitoken { get; set; }

    public string? TrainingFileUrl { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
