using System.Collections.Generic;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core.Domain;

namespace Upico.Core.Services
{
    public interface IUserService
    {
        public Task<string> Authenticate(LoginRequest request);
        public Task<IList<string>> Register(RegisterRequest request);
        public Task<AppUser> GetUser(string userName);
        public Task<List<AppUser>> SearchUser(string key);
        public Task<bool> IsFollowed(string followerUsername, string followingUsername);
    }
}
