namespace Daira.Infrastructure.Specefication
{
    public class FriendshipSpecification : BaseSpecefication<Friendship>
    {
        public FriendshipSpecification() : base()
        {
            AddInculdes();
        }
        public FriendshipSpecification(Guid id) : base(f => f.Id == id)
        {
            AddInculdes();
        }
        public FriendshipSpecification(Expression<Func<Friendship, bool>> expression) : base(expression)
        {
            AddInculdes();
        }
        void AddInculdes()
        {
            Includes.Add(f => f.Requester);
            Includes.Add(F => F.Addressee);
        }
    }
}
