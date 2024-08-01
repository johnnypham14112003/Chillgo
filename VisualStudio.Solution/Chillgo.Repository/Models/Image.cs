namespace Chillgo.Repository.Models;

public partial class Image
{
    public Guid Id { get; set; }

    public string? UrlPath { get; set; }

    public bool IsAvatar { get; set; }

    public byte Type { get; set; }

    public string Status { get; set; } = null!;

    public Guid? AccountId { get; set; }

    public Guid? CertificateId { get; set; }

    public Guid? LocationId { get; set; }

    public Guid? HotelId { get; set; }

    public Guid? TransportId { get; set; }

    public Guid? BlogId { get; set; }

    public Guid? VoucherId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual VerificationRequest? Certificate { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Transport? Transport { get; set; }

    public virtual Voucher? Voucher { get; set; }
}
