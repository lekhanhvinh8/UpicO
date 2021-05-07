using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;
using Upico.Core.Repositories;

namespace Upico.Persistence.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly UpicODbContext _context;

        public LikeRepository(UpicODbContext context)
            :base(context)
        {
            _context = context;
        }
    }
}
