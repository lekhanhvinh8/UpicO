using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Core.Domain;
using Upico.Core.Repositories;

namespace Upico.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UpicODbContext _dbContext;

        public UserRepository(UpicODbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<AppUser> GetUser(string userName)
        {
            return await this._dbContext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
