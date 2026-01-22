namespace Daira.Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(m => m.Content).HasColumnType("nvarchar(max)");
            builder.Property(m => m.IsRead).HasDefaultValue(false);

            //Relation
            builder.HasOne(m => m.Conversation).WithMany(c => c.Messages).HasForeignKey(c => c.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Sender).WithMany(a => a.Messages).HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            //Index
            builder.HasIndex(m => m.SenderId).HasDatabaseName("idx_messages_sender_id");
            builder.HasIndex(m => m.ConversationId).HasDatabaseName("idx_messages_conversation_id");
        }
    }
}
