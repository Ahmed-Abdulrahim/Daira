namespace Daira.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> GetByIdSpec(ISpecefication<T> spec);
        Task<IEnumerable<T>> GetAllWithSpec(ISpecefication<T> spec);
        public Task<T> GetByIdTrackedAsync(int id);
        public Task<T> GetByIdSpecTracked(ISpecefication<T> spec);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
