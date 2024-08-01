namespace Chillgo.Repository.Models;

public partial class Comment
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime SentTime { get; set; }

    public decimal Rating { get; set; }

    public Guid SenderId { get; set; }

    public byte Type { get; set; }

    public string Status { get; set; } = null!;

    public Guid? LocationId { get; set; }

    public Guid? PersonId { get; set; }

    public Guid? BlogId { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Account? Person { get; set; }

    public virtual Account Sender { get; set; } = null!;
}
