namespace Daira.Infrastructure.unitOfWork
{
    public class UnitOfWorkClass(DairaDbContext context, UserManager<AppUser> userManger) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = new();


        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            var key = typeof(T).Name;
            if (!repositories.ContainsKey(key))
            {
                var repo = new GenericRepository<T>(context);
                repositories.Add(key, repo);
            }

            return (IRepository<T>)repositories[key];
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await context.Database.BeginTransactionAsync();

        public async Task<int> CommitAsync() => await context.SaveChangesAsync();


        public void Dispose() => context.Dispose();




    }
}
