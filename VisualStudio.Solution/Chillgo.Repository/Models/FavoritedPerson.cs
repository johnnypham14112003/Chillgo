namespace Chillgo.Repository.Models;

public partial class FavoritedPerson
{
    public Guid AccountId { get; set; }

    public Guid PersonId { get; set; }

    public bool IsTourGuide { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Account Person { get; set; } = null!;
}
