namespace Daira.Infrastructure.Specefication
{
    public class PostSpecification : BaseSpecefication<Post>
    {
        public PostSpecification() : base()
        {
            AddIncludes();
        }
        public PostSpecification(Guid id) : base(p => p.Id == id)
        {
            AddIncludes();
        }
        public PostSpecification(Expression<Func<Post, bool>> expression) : base(expression)
        {
            AddIncludes();
        }


        void AddIncludes()
        {
            Includes.Add(p => p.User);
            Includes.Add(p => p.Comments);
            Includes.Add(p => p.Likes);
        }
    }
}
