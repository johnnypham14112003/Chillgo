namespace Chillgo.Repository.Models;

public partial class Blog
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal TotalRating { get; set; }

    public DateTime PostedDate { get; set; }

    public Guid? AccountId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account? Account { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
