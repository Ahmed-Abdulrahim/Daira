namespace Daira.Infrastructure.Persistence.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasColumnType("NvarChar(100)");
            builder.Property(c => c.Type).HasConversion<string>().IsRequired().HasMaxLength(50);
            builder.Property(c => c.CreatedAt)
              .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.CreatedAt).IsRequired();

            builder.HasOne(c => c.CreatedBy).WithMany().HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(c => c.Participants).WithOne(cp => cp.Conversation).HasForeignKey(cp => cp.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(c => c.Messages).WithOne(m => m.Conversation).HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
