namespace Daira.Infrastructure.Repositories.MainRepository
{
    public class GenericRepository<T>(DairaDbContext context) : IRepository<T> where T : BaseEntity
    {
        private DbSet<T> Set = context.Set<T>();
        public async Task AddAsync(T entity) => await Set.AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities) => await Set.AddRangeAsync(entities);
        public void Delete(T entity) => Set.Remove(entity);
        public void DeleteRange(IEnumerable<T> entities) => Set.RemoveRange(entities);
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) => Set.Where(predicate);
        public async Task<IEnumerable<T>> GetAllAsync() => await Set.AsNoTracking().ToListAsync();
        public async Task<IEnumerable<T>> GetAllWithSpec(ISpecefication<T> spec) => await GenerateQuery(spec).AsNoTracking().ToListAsync();
        public async Task<T> GetByIdAsync(Guid id) => await Set.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        public async Task<T> GetByIdSpec(ISpecefication<T> spec) => await GenerateQuery(spec).AsNoTracking().FirstOrDefaultAsync();
        public async Task<T> GetByIdSpecTracked(ISpecefication<T> spec) => await GenerateQuery(spec).FirstOrDefaultAsync();
        public async Task<T> GetByIdTrackedAsync(Guid id) => await Set.FirstOrDefaultAsync(e => e.Id == id);
        public Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate) => Set.AsNoTracking().FirstOrDefaultAsync(predicate);
        public void Update(T entity) => Set.Update(entity);
        IQueryable<T> GenerateQuery(ISpecefication<T> spec) => SpecificationEvaluator<T>.GetQuery(Set, spec);
    }
}
