
namespace Daira.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.Property(n => n.TargetType).HasConversion<string>()
               .HasColumnType("nvarchar(20)");
            builder.Property(n => n.Type).HasConversion<string>()
               .IsRequired().HasColumnType("nvarchar(30)");
            builder.Property(n => n.Content).HasColumnType("nvarchar(500)");
            builder.Property(n => n.IsRead).HasDefaultValue(false);
            builder.Property(n => n.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            //Relation
            builder.HasOne(a => a.User).WithMany(n => n.ReceivedNotifications).HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Actor).WithMany(n => n.TriggeredNotifications).HasForeignKey(n => n.ActorId)
               .OnDelete(DeleteBehavior.NoAction);
            //Index
            builder.HasIndex(n => n.UserId).HasDatabaseName("idx_notifications_user_id");
            builder.HasIndex(n => new { n.UserId, n.IsRead }).HasDatabaseName("idx_notifications_user_isread");
            builder.HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt })
                  .HasDatabaseName("idx_notifications_user_isread_created");
        }
    }
}
