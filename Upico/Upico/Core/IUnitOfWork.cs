using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Repositories;

namespace Upico.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IAvatarRepository Avatars { get; }

        Task<int> Complete();
    }
}
