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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public UserRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config)
        {            
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            // check user exitst
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return null;

            //check password
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
                return null;

            //get roles
            var roles = await _userManager.GetRolesAsync(user);

            //create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FullName),
                new Claim(ClaimTypes.Name,user.UserName)
            };
            foreach (var i in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, i));
            }

            //create token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            //return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AppUser> GetUser(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<string> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            //check username
            if (user!= null)
            {
                return "Tài khoản đã tồn tại";
            }

            //check email
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return "Email đã dược sử dụng";
            }

            user = new AppUser()
            {
                UserName = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FullName = request.FullName,
                Email = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return null;
            return "lỗi";
        }
    }
}
