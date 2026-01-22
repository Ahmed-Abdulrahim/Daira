namespace Daira.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(r => r.Id);
            // Properties
            builder.Property(r => r.UserId).IsRequired();

            builder.Property(r => r.Token).HasColumnType("nvarchar(500)").IsRequired();

            builder.Property(r => r.ExpiresAt).HasColumnType("datetime2").IsRequired();

            builder.Property(r => r.IsRevoked).HasColumnType("bit").HasDefaultValue(false)
                .IsRequired();

            builder.Property(r => r.CreatedAt).HasColumnType("datetime2").HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            //RelationShips
            builder.HasOne(r => r.AppUser).WithMany(a => a.RefreshTokens)
                .HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);

            //Index
            builder.HasIndex(r => r.Token).HasDatabaseName("idx_refreshtokens_token");
            builder.HasIndex(r => r.UserId).HasDatabaseName("idx_refreshtokens_user_id");
        }
    }
}
