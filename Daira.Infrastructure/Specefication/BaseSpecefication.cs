namespace Daira.Infrastructure.Specefication
{
    public class BaseSpecefication<T> : ISpecefication<T> where T : BaseEntity
    {
        public BaseSpecefication()
        {

        }
        public BaseSpecefication(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public List<string> IncludeStrings { get; set; } = new List<string>();

        public int Take { get; set; }
        public int Skip { get; set; }
        public bool HasPagination { get; set; }


        protected void AddOrderBy(Expression<Func<T, object>> expression)
            => OrderBy = expression;
        protected void AddOrderByDesc(Expression<Func<T, object>> expression)
            => OrderByDesc = expression;


        protected void AddInclude(string includeString)
            => IncludeStrings.Add(includeString);
        protected void AddPagination(int skip, int take)
        {
            HasPagination = true;
            Skip = skip;
            Take = take;
        }
    }
}
