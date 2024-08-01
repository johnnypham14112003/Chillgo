namespace Chillgo.Repository.Models;

public partial class VerificationRequest
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public Guid SenderId { get; set; }

    public Guid StaffVerifyId { get; set; }

    public DateTime SentDate { get; set; }

    public DateTime HandleDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Account Sender { get; set; } = null!;

    public virtual Account StaffVerify { get; set; } = null!;
}
