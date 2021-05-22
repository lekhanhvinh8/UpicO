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
            CreateMap<AppUser, UserResource>();
            CreateMap<PostedImage, PhotoResource>()
                .ForMember(pt => pt.Url, opt => opt.MapFrom(pi => pi.Path));
            CreateMap<Post, PostResouce>();
            CreateMap<Post, DetailedPostResource>()
                .ForMember(dp => dp.Comments, opt => opt.MapFrom(p => p.Comments.Select(pf => pf.Content)))
                .ForMember(dp => dp.Likes, opt => opt.MapFrom(p => p.Likes.Count()))
                .ForMember(dp => dp.DisplayName, opt => opt.MapFrom(p => p.User.DisplayName))
                .ForMember(dp => dp.AvatarUrl, opt => opt.MapFrom(p => p.User.Avatars.FirstOrDefault(a => a.IsMain).Path));

            CreateMap<Comment, CommentDetailResource>()
                .ForMember(cr => cr.Username, opt => opt.MapFrom(c => c.User.UserName))
                .ForMember(cr => cr.UserDisplayName, opt => opt.MapFrom(c => c.User.DisplayName))
                .ForMember(cr => cr.UserAvatarUrl, opt => opt.MapFrom(c => c.User.Avatars.FirstOrDefault(a => a.IsMain).Path));
            CreateMap<Comment, CommentResouce>()
                .ForMember(cr => cr.Childs, opt => opt.Ignore())
                .AfterMap((c, cr) => {
                    MapChildren(c, cr);
                });

            CreateMap<CreatePostResource, Post>();
        }

        private void MapChildren(Comment comment, CommentResouce commentResouce)
        {
            //basic mapping
            commentResouce.Id = comment.Id;
            commentResouce.Content = comment.Content;
            commentResouce.DateCreate = comment.DateCreate;
            commentResouce.Username = comment.User.UserName;
            commentResouce.UserDisplayName = comment.User.DisplayName;

            var avatar = comment.User.Avatars.FirstOrDefault(a => a.IsMain);

            if(avatar != null)
                commentResouce.UserAvatarUrl = avatar.Path;
            else
                commentResouce.UserAvatarUrl = null;

            //Child mapping
            if (comment.Childs.Count == 0)
                return;

            foreach (var child in comment.Childs)
            {
                var commentResourceChild = new CommentResouce();
                MapChildren(child, commentResourceChild);

                commentResouce.Childs.Add(commentResourceChild);
            }

        }
    }
}
