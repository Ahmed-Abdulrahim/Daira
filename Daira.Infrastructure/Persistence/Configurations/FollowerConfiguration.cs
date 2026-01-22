namespace Daira.Infrastructure.Persistence.Configurations
{
    public class FollowerConfiguration : IEntityTypeConfiguration<Follower>
    {
        public void Configure(EntityTypeBuilder<Follower> builder)
        {
            builder.ToTable("Followers");
            builder.HasKey(x => x.Id);
            builder.Property(f => f.FollowerId).IsRequired().HasMaxLength(450);
            builder.Property(f => f.FollowingId).IsRequired().HasMaxLength(450);
            builder.Property(f => f.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

            //Relation
            builder.HasOne(c => c.FollowerUser).WithMany(a => a.Follower).HasForeignKey(a => a.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.FollowingUser).WithMany(a => a.Following).HasForeignKey(a => a.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
            //Unique
            builder.HasIndex(f => new { f.FollowerId, f.FollowingId }).IsUnique().HasDatabaseName("UQ_Followers");


            //Index
            builder.HasIndex(c => c.FollowingId).HasDatabaseName("idx_followers_following_id");
            builder.HasIndex(c => c.FollowerId).HasDatabaseName("idx_followers_follower_id");
        }
    }
}
