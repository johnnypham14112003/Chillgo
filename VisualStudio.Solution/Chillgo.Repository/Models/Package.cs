namespace Chillgo.Repository.Models;

public partial class Package
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public short Duration { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PackageTransaction> PackageTransactions { get; set; } = new List<PackageTransaction>();
}
