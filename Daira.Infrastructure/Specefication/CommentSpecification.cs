namespace Daira.Infrastructure.Specefication
{
    public class CommentSpecification : BaseSpecefication<Comment>
    {
        public CommentSpecification() : base()
        {
            AddIncludes();
        }
        public CommentSpecification(Guid id) : base(c => c.Id == id)
        {
            AddIncludes();
        }
        public CommentSpecification(Expression<Func<Comment, bool>> expression) : base(expression)
        {
            AddIncludes();
        }
        void AddIncludes()
        {
            Includes.Add(c => c.AppUser);
            Includes.Add(c => c.Post);
        }
    }
}
