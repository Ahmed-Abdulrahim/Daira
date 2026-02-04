namespace Daira.Infrastructure.Specefication
{
    public class ConversationParticipantSpecification : BaseSpecefication<ConversationParticipant>
    {
        public ConversationParticipantSpecification() : base()
        {
            AddIncludes();
        }

        public ConversationParticipantSpecification(Guid id) : base(f => f.Id == id)
        {
            AddIncludes();
        }
        public ConversationParticipantSpecification(Expression<Func<ConversationParticipant, bool>> expression) : base(expression)
        {
            AddIncludes();
        }
        void AddIncludes()
        {
            Includes.Add(c => c.AppUser);
            Includes.Add(c => c.Conversation);

        }
    }
}
