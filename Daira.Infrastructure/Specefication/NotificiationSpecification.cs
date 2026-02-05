namespace Daira.Infrastructure.Specefication
{
    public class NotificiationSpecification : BaseSpecefication<Notification>
    {
        public NotificiationSpecification() : base()
        {
            AddIncludes();
        }

        public NotificiationSpecification(Guid id) : base(f => f.Id == id)
        {
            AddIncludes();
        }
        public NotificiationSpecification(Expression<Func<Notification, bool>> expression) : base(expression)
        {
            AddIncludes();
        }
        public NotificiationSpecification(Expression<Func<Notification, bool>> expression, int pageNumber, int pageSize) : base(expression)
        {
            AddIncludes();
            AddPagination((pageNumber - 1) * pageSize, pageSize);
        }
        void AddIncludes()
        {
            Includes.Add(f => f.Actor);
            Includes.Add(f => f.User);
        }
    }
}
