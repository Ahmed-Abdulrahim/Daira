namespace Daira.Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Content).HasColumnType("nvarchar(max)");
            builder.Property(p => p.ImageUrl).HasColumnType("nvarchar(500)");
            builder.Property(p => p.LikesCount).HasDefaultValue(0);
            builder.Property(p => p.CommentsCount).HasDefaultValue(0);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");


            //Relationships
            builder.HasOne(p => p.User).WithMany(u => u.Posts).HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Index
            builder.HasIndex(p => p.UserId).HasDatabaseName("idx_posts_user_id");
            builder.HasIndex(p => p.CreatedAt).HasDatabaseName("idx_posts_created_at");
        }
    }
}
