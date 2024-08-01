namespace Chillgo.Repository.Models;

public partial class Schedule
{
    public Guid Id { get; set; }

    public Guid PlanId { get; set; }

    public string Content { get; set; } = null!;

    public decimal EstimatedCost { get; set; }

    public DateTime TimeStamp { get; set; }

    public Guid? LocationId { get; set; }

    public Guid? HotelId { get; set; }

    public Guid? TransportId { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Plan Plan { get; set; } = null!;

    public virtual Transport? Transport { get; set; }
}
