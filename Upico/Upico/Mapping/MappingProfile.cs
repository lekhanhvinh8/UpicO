using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core.Domain;

namespace Upico.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Avatar, AvatarResource>();

            CreateMap<CreatePostResource, Post>();
            CreateMap<Post, PostResouce>();
        }
    }
}
