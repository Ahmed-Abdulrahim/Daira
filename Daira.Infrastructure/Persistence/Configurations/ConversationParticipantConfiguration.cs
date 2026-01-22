namespace Daira.Infrastructure.Persistence.Configurations
{
    public class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.ToTable("ConversationParticipants");
            builder.HasKey(x => x.Id);

            //relationships
            builder.HasOne(c => c.Conversation).WithMany(con => con.Participants).HasForeignKey(c => c.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(u => u.AppUser).WithMany(a => a.ConversationParticipants).HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            //Unique Constraint
            builder.HasIndex(c => new { c.ConversationId, c.UserId }).IsUnique()
                .HasDatabaseName("UQ_ConversationParticipants");

            //Index
            builder.HasIndex(c => c.UserId).HasDatabaseName("idx_conv_participants_user_id");
        }
    }
}
