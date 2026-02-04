namespace Daira.Application.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> GetByIdSpec(ISpecefication<T> spec);
        Task<IEnumerable<T>> GetAllWithSpec(ISpecefication<T> spec, CancellationToken cancellationToken = default);
        public Task<T> GetByIdTrackedAsync(Guid id);
        public Task<T> GetByIdSpecTracked(ISpecefication<T> spec, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
