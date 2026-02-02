namespace Daira.Infrastructure.Specefication
{
    public class LikeSpecification : BaseSpecefication<Like>
    {
        public LikeSpecification() : base()
        {
            AddIncludes();
        }
        public LikeSpecification(Guid id) : base(l => l.Id == id)
        {
            AddIncludes();
        }
        public LikeSpecification(Expression<Func<Like, bool>> expression) : base(expression)
        {
            AddIncludes();
        }
        void AddIncludes()
        {
            Includes.Add(l => l.Post);
            Includes.Add(l => l.AppUser);
        }
    }
}
