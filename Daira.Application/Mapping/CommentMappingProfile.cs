namespace Daira.Application.Mapping
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<CommentDto, Comment>().ReverseMap();
        }
    }
}
