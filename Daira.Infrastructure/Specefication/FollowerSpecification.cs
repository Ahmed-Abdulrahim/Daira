namespace Daira.Infrastructure.Specefication
{
    public class FollowerSpecification : BaseSpecefication<Follower>
    {
        public FollowerSpecification() : base()
        {
            AddIncludes();
        }

        public FollowerSpecification(Guid id) : base(f => f.Id == id)
        {
            AddIncludes();
        }
        public FollowerSpecification(Expression<Func<Follower, bool>> expression) : base(expression)
        {
            AddIncludes();
        }
        void AddIncludes()
        {
            Includes.Add(f => f.FollowerUser);
            Includes.Add(f => f.FollowingUser);
        }
    }
}
