using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;

namespace Upico.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<AppUser> GetUser(string userName);
    }
}
