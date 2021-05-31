using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core.Domain;
using Upico.Core.Repositories;

namespace Upico.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UpicODbContext _context;

        public UserRepository(UpicODbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetUser(string userName)
        {
            var user = await this._context.Users.SingleOrDefaultAsync(u => u.UserName == userName);

            return user;
        }

        public async Task<AppUser> GetUserWithLikes(string userName)
        {
            var user = await this._context.Users
                .Include(u => u.Likes)
                .SingleOrDefaultAsync(u => u.UserName == userName);

            return user;
        }

        public async Task<AppUser> SearchUserByUsername(string username)
        {
            var user = await this._context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            
            return user;
        }

        public async Task<AppUser> SearchUserById(string id)
        {
            var user = await this._context.Users.SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<List<AppUser>> SearchUsersByDisplayName(string displayName)
        {
            var users = await this._context.Users.Where(u => u.DisplayName.Contains(displayName)).ToListAsync();

            return users;
        }

        public async Task LoadMainAvatar(string userName)
        {
            await this._context.Avatars.Where(a => a.IsMain && a.AppUser.UserName == userName).LoadAsync();
        }
    }
}
