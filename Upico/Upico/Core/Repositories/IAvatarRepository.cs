﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;

namespace Upico.Core.Repositories
{
    public interface IAvatarRepository : IRepository<Avatar>
    {
        public Task<List<Avatar>> GetAvatar(string userName);
    }
}
