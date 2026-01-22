
namespace Daira.Infrastructure.Persistence.Configurations
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendships");
            builder.HasKey(f => f.Id);
            builder.Property(f => f.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(f => f.Status).HasConversion<string>().HasDefaultValue(RequestStatus.Pending);

            //RelationShip
            builder.HasOne(f => f.Requester).WithMany(a => a.SentFriendRequests).HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Addressee).WithMany(a => a.ReceivedFriendRequests).HasForeignKey(f => f.AddresseeId)
               .OnDelete(DeleteBehavior.NoAction);

            //Unique
            builder.HasIndex(f => new { f.RequesterId, f.AddresseeId }).IsUnique()
                .HasDatabaseName("UQ_Friendships");

            //Index
            builder.HasIndex(f => f.RequesterId)
               .HasDatabaseName("idx_friendships_requester_id");

            builder.HasIndex(f => f.AddresseeId)
               .HasDatabaseName("idx_friendships_addressee_id");

        }
    }
}
