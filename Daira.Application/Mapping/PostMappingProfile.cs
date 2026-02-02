namespace Daira.Application.Mapping
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<AppUser, AuthorResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.PictureUrl));


            CreateMap<Post, CreatePostResponse>()
                .ForMember(dest => dest.Succeeded, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.LikesCount))
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.CommentsCount))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(_ => "Post Created successfully."))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.IsLikedByMe, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.Errors, opt => opt.Ignore());

            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Succeeded, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.LikesCount))
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.CommentsCount))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Message, opt => opt.Ignore())
                .ForMember(dest => dest.Errors, opt => opt.Ignore());

            CreateMap<Post, UpdateResponse>()
               .ForMember(dest => dest.Succeeded, opt => opt.MapFrom(_ => true))
               .ForMember(dest => dest.Message, opt => opt.MapFrom(_ => "Post updated successfully."))
               .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User))
               .ForMember(dest => dest.Errors, opt => opt.Ignore());


        }
    }
}