
namespace Daira.Infrastructure.Persistence.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Likes");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            //Relation
            builder.HasOne(l => l.AppUser).WithMany(a => a.Likes).HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(l => l.Post).WithMany(a => a.Likes).HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);


            //Unique
            builder.HasIndex(l => new { l.UserId, l.PostId }).IsUnique()
                .HasDatabaseName("UQ_Likes_User_Post");
            //Index
            builder.HasIndex(l => l.PostId).HasDatabaseName("idx_likes_post_id");
            builder.HasIndex(l => l.UserId).HasDatabaseName("idx_likes_user_id");
        }
    }
}
