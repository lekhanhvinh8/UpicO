using AutoMapper;
using System.Linq;
using Upico.Controllers.Resouces;
using Upico.Controllers.Resources;
using Upico.Core.Domain;

namespace Upico.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Avatar, AvatarResource>();
            CreateMap<PostedImage, PhotoResource>()
                .ForMember(pt => pt.Url, opt => opt.MapFrom(pi => pi.Path));

            CreateMap<Post, PostResouce>();
            CreateMap<Post, DetailedPostResource>()
                .ForMember(dp => dp.Comments, opt => opt.MapFrom(p => p.Comments.Take(3).Select(pf => pf.Content)))
                .ForMember(dp => dp.Likes, opt => opt.MapFrom(p => p.Likes.Count()));
            

            CreateMap<CreatePostResource, Post>();
        }
    }
}
