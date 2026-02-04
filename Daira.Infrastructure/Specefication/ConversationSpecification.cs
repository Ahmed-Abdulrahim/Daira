namespace Daira.Infrastructure.Specefication
{
    public class ConversationSpecification : BaseSpecefication<Conversation>
    {
        public ConversationSpecification() : base()
        {
            AddIncludes();
        }

        public ConversationSpecification(Guid id) : base(f => f.Id == id)
        {
            AddIncludes();
        }
        public ConversationSpecification(Expression<Func<Conversation, bool>> expression) : base(expression)
        {
            AddIncludes();
        }
        void AddIncludes()
        {
            Includes.Add(c => c.Messages);
            Includes.Add(c => c.Participants);
            Includes.Add(c => c.CreatedBy);
            AddInclude("Participants.AppUser");
        }
    }
}
