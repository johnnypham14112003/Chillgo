using Chillgo.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Chillgo.Repository;

public class ChillgoDbContext : DbContext
{
    public ChillgoDbContext()
    {
    }

    public ChillgoDbContext(DbContextOptions<ChillgoDbContext> options)
        : base(options)
    {
    }


    public DbSet<Account> Accounts { get; set; }

    public  DbSet<Blog> Blogs { get; set; }

    public  DbSet<Booking> Bookings { get; set; }

    public  DbSet<BookingDetail> BookingDetails { get; set; }

    public  DbSet<BotAi> BotAis { get; set; }

    public  DbSet<ChillCoinTask> ChillCoinTasks { get; set; }

    public  DbSet<Comment> Comments { get; set; }

    public  DbSet<Conversation> Conversations { get; set; }

    public  DbSet<CustomerChillCoinTask> CustomerChillCoinTasks { get; set; }

    public  DbSet<CustomerVoucher> CustomerVouchers { get; set; }

    public  DbSet<FavoritedPerson> FavoritedPeople { get; set; }

    public  DbSet<Hobby> Hobbies { get; set; }

    public  DbSet<Hotel> Hotels { get; set; }

    public  DbSet<HotelRoom> HotelRooms { get; set; }

    public  DbSet<Image> Images { get; set; }

    public  DbSet<Location> Locations { get; set; }

    public  DbSet<Message> Messages { get; set; }

    public  DbSet<Package> Packages { get; set; }

    public  DbSet<PackageTransaction> PackageTransactions { get; set; }

    public  DbSet<Plan> Plans { get; set; }

    public  DbSet<SalaryTransaction> SalaryTransactions { get; set; }

    public  DbSet<Schedule> Schedules { get; set; }

    public  DbSet<Transport> Transports { get; set; }

    public  DbSet<VerificationRequest> VerificationRequests { get; set; }

    public  DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC0777DB94D0");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "Idx_Account_AccountEmail");

            entity.HasIndex(e => e.Cccd, "Idx_Account_CCCD");

            entity.HasIndex(e => e.PhoneNumber, "Idx_Account_PhoneNumber");

            entity.HasIndex(e => e.Status, "Idx_Account_Status");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D10534F2AB258C").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Cccd)
                .HasMaxLength(20)
                .HasColumnName("CCCD");
            entity.Property(e => e.CompanyName).HasMaxLength(150);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.JoinedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .HasDefaultValue("Người Dùng");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chưa Xác Thực");

            entity.HasMany(d => d.Hotels).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoritedHotel",
                    r => r.HasOne<Hotel>().WithMany()
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favorited__Hotel__245D67DE"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favorited__Accou__236943A5"),
                    j =>
                    {
                        j.HasKey("AccountId", "HotelId").HasName("PK__Favorite__D0FD861B9DF21605");
                        j.ToTable("FavoritedHotel");
                        j.HasIndex(new[] { "AccountId" }, "Idx_FavoritedHotel_AccountId");
                    });

            entity.HasMany(d => d.Locations).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoritedLocation",
                    r => r.HasOne<Location>().WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favorited__Locat__1BC821DD"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favorited__Accou__1AD3FDA4"),
                    j =>
                    {
                        j.HasKey("AccountId", "LocationId").HasName("PK__Favorite__5AE24FEFAFEE226A");
                        j.ToTable("FavoritedLocation");
                        j.HasIndex(new[] { "AccountId" }, "Idx_FavoritedLocation_AccountId");
                    });

            entity.HasMany(d => d.Transports).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoritedTransport",
                    r => r.HasOne<Transport>().WithMany()
                        .HasForeignKey("TransportId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favorited__Trans__282DF8C2"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favorited__Accou__2739D489"),
                    j =>
                    {
                        j.HasKey("AccountId", "TransportId").HasName("PK__Favorite__F5033FB7FF38059D");
                        j.ToTable("FavoritedTransport");
                        j.HasIndex(new[] { "AccountId" }, "Idx_FavoritedTransport_AccountId");
                    });
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blog__3214EC070F78D921");

            entity.ToTable("Blog");

            entity.HasIndex(e => e.AccountId, "Idx_Blog_AccountId");

            entity.HasIndex(e => e.PostedDate, "Idx_Blog_PostedDate");

            entity.HasIndex(e => e.Status, "Idx_Blog_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PostedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Đăng");
            entity.Property(e => e.TotalRating).HasColumnType("decimal(2, 1)");

            entity.HasOne(d => d.Account).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Blog__AccountId__693CA210");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC07998A5551");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.Status, "Idx_Booking_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BookedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).HasDefaultValue("Trống!");
            entity.Property(e => e.PayMethod)
                .HasMaxLength(30)
                .HasDefaultValue("Tiền Mặt");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Giỏ Hàng");
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Account).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__Account__31B762FC");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK__Booking__Voucher__32AB8735");
        });

        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingD__3214EC072F51946E");

            entity.ToTable("BookingDetail");

            entity.HasIndex(e => e.HotelId, "Idx_BookingDetail_HotelId");

            entity.HasIndex(e => e.LocationId, "Idx_BookingDetail_LocationId");

            entity.HasIndex(e => e.Status, "Idx_BookingDetail_Status");

            entity.HasIndex(e => e.TourGuideId, "Idx_BookingDetail_TourGuideId");

            entity.HasIndex(e => e.TransportId, "Idx_BookingDetail_TransportId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.RoomType).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Giỏ Hàng");
            entity.Property(e => e.Subtotal).HasColumnType("money");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingDe__Booki__367C1819");

            entity.HasOne(d => d.Hotel).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__BookingDe__Hotel__3D2915A8");

            entity.HasOne(d => d.Location).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__BookingDe__Locat__3C34F16F");

            entity.HasOne(d => d.Room).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__BookingDe__RoomI__3F115E1A");

            entity.HasOne(d => d.TourGuide).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.TourGuideId)
                .HasConstraintName("FK__BookingDe__TourG__3B40CD36");

            entity.HasOne(d => d.Transport).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.TransportId)
                .HasConstraintName("FK__BookingDe__Trans__3E1D39E1");

            entity.HasOne(d => d.Voucher).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK__BookingDe__Vouch__40058253");
        });

        modelBuilder.Entity<BotAi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BotAI__3214EC072925B359");

            entity.ToTable("BotAI");

            entity.HasIndex(e => e.Status, "Idx_BotAI_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Apiendpoint).HasColumnName("APIEndpoint");
            entity.Property(e => e.Apitoken).HasColumnName("APIToken");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Provider).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Khả Dụng");
            entity.Property(e => e.TrainingFileUrl).HasColumnName("TrainingFileURL");
        });

        modelBuilder.Entity<ChillCoinTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChillCoi__3214EC074AFA3B4C");

            entity.ToTable("ChillCoinTask");

            entity.HasIndex(e => e.Status, "Idx_ChillCoinTask_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.RewardCoin).HasDefaultValue(1);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Khả Dụng");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC0709731265");

            entity.ToTable("Comment");

            entity.HasIndex(e => e.BlogId, "Idx_Comment_BlogId");

            entity.HasIndex(e => e.LocationId, "Idx_Comment_LocationId");

            entity.HasIndex(e => e.PersonId, "Idx_Comment_PersonId");

            entity.HasIndex(e => e.Status, "Idx_Comment_Status");

            entity.HasIndex(e => e.Type, "Idx_Comment_Type");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.SentTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Gửi");
            entity.Property(e => e.Type).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__Comment__BlogId__74AE54BC");

            entity.HasOne(d => d.Location).WithMany(p => p.Comments)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Comment__Locatio__72C60C4A");

            entity.HasOne(d => d.Person).WithMany(p => p.CommentPeople)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK__Comment__PersonI__73BA3083");

            entity.HasOne(d => d.Sender).WithMany(p => p.CommentSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__SenderI__6FE99F9F");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC0702D83615");

            entity.ToTable("Conversation");

            entity.HasIndex(e => e.AibotId, "Idx_Conversation_AIBotId");

            entity.HasIndex(e => e.FirstAccountId, "Idx_Conversation_FirstAccountId");

            entity.HasIndex(e => e.SecondAccountId, "Idx_Conversation_SecondAccountId");

            entity.HasIndex(e => e.Status, "Idx_Conversation_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AibotId).HasColumnName("AIBotId");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsHuman).HasDefaultValue(true);
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SecondName).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chờ Xác Nhận");

            entity.HasOne(d => d.Aibot).WithMany(p => p.Conversations)
                .HasForeignKey(d => d.AibotId)
                .HasConstraintName("FK__Conversat__AIBot__671F4F74");

            entity.HasOne(d => d.FirstAccount).WithMany(p => p.ConversationFirstAccounts)
                .HasForeignKey(d => d.FirstAccountId)
                .HasConstraintName("FK__Conversat__First__65370702");

            entity.HasOne(d => d.SecondAccount).WithMany(p => p.ConversationSecondAccounts)
                .HasForeignKey(d => d.SecondAccountId)
                .HasConstraintName("FK__Conversat__Secon__662B2B3B");
        });

        modelBuilder.Entity<CustomerChillCoinTask>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.ChillCoinTaskId }).HasName("PK__Customer__B9BEB74795715BB7");

            entity.ToTable("CustomerChillCoinTask");

            entity.HasIndex(e => e.Status, "Idx_CustomerChillCoinTask_Status");

            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Làm Mới");

            entity.HasOne(d => d.Account).WithMany(p => p.CustomerChillCoinTasks)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerC__Accou__7E37BEF6");

            entity.HasOne(d => d.ChillCoinTask).WithMany(p => p.CustomerChillCoinTasks)
                .HasForeignKey(d => d.ChillCoinTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerC__Chill__7D439ABD");
        });

        modelBuilder.Entity<CustomerVoucher>(entity =>
        {
            entity.HasKey(e => new { e.VoucherId, e.AccountId }).HasName("PK__Customer__49A7A37B3A51FDCA");

            entity.ToTable("CustomerVoucher");

            entity.HasIndex(e => e.AccountId, "Idx_CustomerVoucher_CustomerId");

            entity.HasIndex(e => e.Status, "Idx_CustomerVoucher_Status");

            entity.Property(e => e.CollectedDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Nhận");

            entity.HasOne(d => d.Account).WithMany(p => p.CustomerVouchers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerV__Accou__59FA5E80");

            entity.HasOne(d => d.Voucher).WithMany(p => p.CustomerVouchers)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerV__Vouch__59063A47");
        });

        modelBuilder.Entity<FavoritedPerson>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.PersonId }).HasName("PK__Favorite__7E3F5A18B341690B");

            entity.ToTable("FavoritedPerson");

            entity.HasIndex(e => e.AccountId, "Idx_FavoritedPerson_AccountId");

            entity.HasOne(d => d.Account).WithMany(p => p.FavoritedPersonAccounts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorited__Accou__1EA48E88");

            entity.HasOne(d => d.Person).WithMany(p => p.FavoritedPersonPeople)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorited__Perso__1F98B2C1");
        });

        modelBuilder.Entity<Hobby>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hobby__3214EC070FE2E9AE");

            entity.ToTable("Hobby");

            entity.HasIndex(e => e.Name, "UQ__Hobby__737584F675FF0656").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasMany(d => d.Accounts).WithMany(p => p.Hobbies)
                .UsingEntity<Dictionary<string, object>>(
                    "HobbyCustomer",
                    r => r.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__HobbyCust__Accou__4F7CD00D"),
                    l => l.HasOne<Hobby>().WithMany()
                        .HasForeignKey("HobbyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__HobbyCust__Hobby__4E88ABD4"),
                    j =>
                    {
                        j.HasKey("HobbyId", "AccountId").HasName("PK__HobbyCus__79F7D195FEDE993B");
                        j.ToTable("HobbyCustomer");
                        j.HasIndex(new[] { "AccountId" }, "Idx_HobbyCustomer_AccountId");
                    });
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hotel__3214EC07D27EAD32");

            entity.ToTable("Hotel");

            entity.HasIndex(e => e.Hotline, "Idx_Hotel_Hotline");

            entity.HasIndex(e => e.Status, "Idx_Hotel_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Hotline)
                .HasMaxLength(15)
                .HasDefaultValue("Trống");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PriceRange)
                .HasMaxLength(200)
                .HasDefaultValue("0-0");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Còn Phòng");
            entity.Property(e => e.TotalRating).HasColumnType("decimal(2, 1)");
        });

        modelBuilder.Entity<HotelRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HotelRoo__3214EC07A33278C2");

            entity.ToTable("HotelRoom");

            entity.HasIndex(e => e.HotelId, "Idx_HotelRoom_HotelId");

            entity.HasIndex(e => e.Status, "Idx_HotelRoom_Status");

            entity.HasIndex(e => e.Type, "Idx_HotelRoom_Type");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Floor).HasMaxLength(30);
            entity.Property(e => e.PricePerNight).HasColumnType("money");
            entity.Property(e => e.RoomNo).HasMaxLength(30);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Sẵn Sàng");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasDefaultValue("Giường Đơn");

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelRooms)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HotelRoom__Hotel__0B91BA14");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC076D1EB82C");

            entity.ToTable("Image");

            entity.HasIndex(e => e.AccountId, "Idx_Image_AccountId");

            entity.HasIndex(e => e.BlogId, "Idx_Image_BlogId");

            entity.HasIndex(e => e.HotelId, "Idx_Image_HotelId");

            entity.HasIndex(e => e.LocationId, "Idx_Image_LocationId");

            entity.HasIndex(e => e.TransportId, "Idx_Image_TransportId");

            entity.HasIndex(e => e.VoucherId, "Idx_Image_VoucherId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Tải Lên");
            entity.Property(e => e.Type).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Account).WithMany(p => p.Images)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Image__AccountId__0880433F");

            entity.HasOne(d => d.Blog).WithMany(p => p.Images)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__Image__BlogId__0D44F85C");

            entity.HasOne(d => d.Certificate).WithMany(p => p.Images)
                .HasForeignKey(d => d.CertificateId)
                .HasConstraintName("FK__Image__Certifica__09746778");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Images)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__Image__HotelId__0B5CAFEA");

            entity.HasOne(d => d.Location).WithMany(p => p.Images)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Image__LocationI__0A688BB1");

            entity.HasOne(d => d.Transport).WithMany(p => p.Images)
                .HasForeignKey(d => d.TransportId)
                .HasConstraintName("FK__Image__Transport__0C50D423");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Images)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK__Image__VoucherId__0E391C95");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07175B9065");

            entity.ToTable("Location");

            entity.HasIndex(e => e.Coordinates, "Idx_Location_Coordinates");

            entity.HasIndex(e => e.IsMarketingPaid, "Idx_Location_IsMarketingPaid");

            entity.HasIndex(e => e.PartnerId, "Idx_Location_PartnerId");

            entity.HasIndex(e => e.Status, "Idx_Location_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Coordinates).HasMaxLength(100);
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Khả Dụng");
            entity.Property(e => e.TicketPrice).HasColumnType("money");
            entity.Property(e => e.TotalRating).HasColumnType("decimal(2, 1)");

            entity.HasOne(d => d.Partner).WithMany(p => p.LocationsNavigation)
                .HasForeignKey(d => d.PartnerId)
                .HasConstraintName("FK__Location__Partne__619B8048");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC07EE4D29B3");

            entity.ToTable("Message");

            entity.HasIndex(e => e.BotReplyId, "Idx_Message_BotReplyId");

            entity.HasIndex(e => e.SenderId, "Idx_Message_SenderId");

            entity.HasIndex(e => e.Status, "Idx_Message_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SentTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chưa Gửi");

            entity.HasOne(d => d.BotReply).WithMany(p => p.Messages)
                .HasForeignKey(d => d.BotReplyId)
                .HasConstraintName("FK__Message__BotRepl__70A8B9AE");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Message__Convers__6CD828CA");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__Message__SenderI__6FB49575");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Package__3214EC0797A6ED1C");

            entity.ToTable("Package");

            entity.HasIndex(e => e.Status, "Idx_Package_Status");

            entity.HasIndex(e => e.Code, "UQ__Package__A25C5AA7BE171089").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đang Bán");
        });

        modelBuilder.Entity<PackageTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PackageT__3214EC07ED07195E");

            entity.ToTable("PackageTransaction");

            entity.HasIndex(e => e.AccountId, "Idx_PackageTransaction_AccountId");

            entity.HasIndex(e => e.PackageId, "Idx_PackageTransaction_PackageId");

            entity.HasIndex(e => e.Status, "Idx_PackageTransaction_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.PaidAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PayMethod)
                .HasMaxLength(30)
                .HasDefaultValue("Tiền Mặt");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Mua");
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Account).WithMany(p => p.PackageTransactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PackageTr__Accou__4B7734FF");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageTransactions)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PackageTr__Packa__4C6B5938");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Plan__3214EC0774A2365C");

            entity.ToTable("Plan");

            entity.HasIndex(e => e.AccountId, "Idx_Plan_AccountId");

            entity.HasIndex(e => e.Status, "Idx_Plan_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Tạo");
            entity.Property(e => e.TotalCost).HasColumnType("money");

            entity.HasOne(d => d.Account).WithMany(p => p.Plans)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Plan__AccountId__756D6ECB");
        });

        modelBuilder.Entity<SalaryTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SalaryTr__3214EC075B5466F3");

            entity.ToTable("SalaryTransaction");

            entity.HasIndex(e => e.AccountId, "Idx_SalaryTransaction_AccountId");

            entity.HasIndex(e => e.PaidAt, "Idx_SalaryTransaction_PaidAt");

            entity.HasIndex(e => e.Status, "Idx_SalaryTransaction_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BaseSalary).HasColumnType("money");
            entity.Property(e => e.Bonus).HasColumnType("money");
            entity.Property(e => e.PaidAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PayMethod)
                .HasMaxLength(30)
                .HasDefaultValue("Tiền Mặt");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Chuyển Trả");
            entity.Property(e => e.TotalPaid).HasColumnType("money");

            entity.HasOne(d => d.Account).WithMany(p => p.SalaryTransactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SalaryTra__Accou__55F4C372");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC07063DD3E7");

            entity.ToTable("Schedule");

            entity.HasIndex(e => e.PlanId, "Idx_Schedule_PlanId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.EstimatedCost).HasColumnType("money");
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("FK__Schedule__HotelI__7FEAFD3E");

            entity.HasOne(d => d.Location).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Schedule__Locati__7EF6D905");

            entity.HasOne(d => d.Plan).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__PlanId__7C1A6C5A");

            entity.HasOne(d => d.Transport).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TransportId)
                .HasConstraintName("FK__Schedule__Transp__00DF2177");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transpor__3214EC0775DB14B4");

            entity.ToTable("Transport");

            entity.HasIndex(e => e.Status, "Idx_Transport_Status");

            entity.HasIndex(e => e.Type, "Idx_Transport_Type");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.LuggageSlot).HasDefaultValue((byte)1);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PricePerDay).HasColumnType("money");
            entity.Property(e => e.Provider).HasMaxLength(100);
            entity.Property(e => e.SitSlot).HasDefaultValue((byte)1);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Sẵn Sàng");
            entity.Property(e => e.TotalRating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasDefaultValue("Xe Ô Tô");
        });

        modelBuilder.Entity<VerificationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Verifica__3214EC07B196C518");

            entity.ToTable("VerificationRequest");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Content).HasDefaultValue("Trống");
            entity.Property(e => e.HandleDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chưa Duyệt");
            entity.Property(e => e.Title).HasDefaultValue("Không có tiêu đề");

            entity.HasOne(d => d.Sender).WithMany(p => p.VerificationRequestSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Verificat__Sende__440B1D61");

            entity.HasOne(d => d.StaffVerify).WithMany(p => p.VerificationRequestStaffVerifies)
                .HasForeignKey(d => d.StaffVerifyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Verificat__Staff__44FF419A");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voucher__3214EC0709EDF0E6");

            entity.ToTable("Voucher");

            entity.HasIndex(e => e.Status, "Idx_Voucher_Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AvailableDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExchangeCode).HasMaxLength(100);
            entity.Property(e => e.ExpiredDate).HasColumnType("datetime");
            entity.Property(e => e.MinimumTransaction).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Khả Dụng");
        });


        base.OnModelCreating(modelBuilder);
    }
}
