namespace Daira.Infrastructure.Specefication
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecefication<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria is not null) // To Add Criteria TO Query If Exists
                query = query.Where(spec.Criteria);


            // To Sort Data 
            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            // Pagenation
            if (spec.HasPagination)
                query = query.Skip(spec.Skip).Take(spec.Take);


            // Now Add Includes To Current Query 
            query = spec.Includes
                .Aggregate(query, (currentQuery, IncludeExpression)
                => currentQuery.Include(IncludeExpression));

            // Add string-based includes (for nested)
            query = spec.IncludeStrings
                .Aggregate(query, (currentQuery, includeString)
                => currentQuery.Include(includeString));


            return query;
        }
    }
}
