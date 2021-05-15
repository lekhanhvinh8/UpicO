using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upico.Controllers.Resources;
using Upico.Core.Domain;

namespace Upico.Core.Services
{
    public interface IUserService
    {
        public Task<string> Authenticate(LoginRequest request);
        public Task<string> Register(RegisterRequest request);
        public Task<AppUser> GetUser(string userName);
    }
}
