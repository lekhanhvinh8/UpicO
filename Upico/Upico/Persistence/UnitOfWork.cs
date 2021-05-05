using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core;
using Upico.Core.Repositories;
using Upico.Persistence.Repositories;

namespace Upico.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UpicODbContext _context;
        public UnitOfWork(UpicODbContext context)
        {
            this._context = context;

            Avatars = new AvatarRepository(_context);
        }

        public IAvatarRepository Avatars { get; private set; }

        public async Task<int> Complete()
        {
            return await this._context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
