namespace Daira.Infrastructure.Specefication
{
    public class FeedSpecification : BaseSpecefication<Post>
    {
        public FeedSpecification(List<string> followedUserIds, int pageNumber = 1, int pageSize = 10)
          : base(p => followedUserIds.Contains(p.UserId))
        {
            AddIncludes();
            AddOrderByDesc(p => p.CreatedAt);
            AddPagination((pageNumber - 1) * pageSize, pageSize);
        }

        private void AddIncludes()
        {
            Includes.Add(p => p.User);
            Includes.Add(p => p.Comments);
            Includes.Add(p => p.Likes);
        }
    }
}
