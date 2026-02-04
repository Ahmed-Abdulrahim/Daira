namespace Daira.Infrastructure.Specefication
{
    public class MessageSpecification : BaseSpecefication<Message>
    {
        public MessageSpecification() : base()
        {
            AddIncludes();
        }

        public MessageSpecification(Guid id) : base(f => f.Id == id)
        {
            AddIncludes();
        }
        public MessageSpecification(Expression<Func<Message, bool>> expression) : base(expression)
        {
            AddIncludes();
        }
        void AddIncludes()
        {
            Includes.Add(f => f.Sender);
            Includes.Add(f => f.Conversation);
        }
    }
}
