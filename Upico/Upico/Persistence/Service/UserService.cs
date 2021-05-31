using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
using Upico.Core;
using Upico.Core.Domain;
using Upico.Core.Services;
using Upico.Core.StaticValues;

namespace Upico.Persistence.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public UserService(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IUnitOfWork unitOfWork,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
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

            var fullName = user.FullName;
            if (fullName == null)
                fullName = "undefined";
            //create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName, fullName),
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
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: creds);

            //return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AppUser> GetUser(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IList<string>> Register(RegisterRequest request)
        {

            var listError = new List<string>();

            var user = await _userManager.FindByNameAsync(request.Username);

            //check email
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                listError.Add("Email already in use");
            }

            //check username
            if (user != null)
            {
                listError.Add("Username already exists");
            }

            user = new AppUser()
            {
                UserName = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FullName = request.FullName,
                Email = request.Email
            };

            if (listError.Count != 0)
                return listError;

            var result = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, RoleNames.RoleUser);

            if (result.Succeeded)
                return null;

            throw new Exception("Error when creating user!! oh yeah");
        }

        public async Task<List<AppUser>> SearchUser(string key)
        {
            var users = new List<AppUser>();

            var user = await this._unitOfWork.Users.SearchUserById(key);
            if (user != null)
            {
                users.Add(user);
            }

            user = await this._unitOfWork.Users.SearchUserByUsername(key);
            if (user != null)
            {
                users.Add(user);
            }

            if (users.Count == 1)
            {
                if(await IsOnlyRoleUser(users[0]))
                {
                    await this._unitOfWork.Users.LoadMainAvatar(users[0].UserName);

                    return users;
                }    

            }

            users = await this._unitOfWork.Users.SearchUsersByDisplayName(key);

            var usersRoleUser = new List<AppUser>();
            foreach (var user1 in users)
            {
                if(await IsOnlyRoleUser(user1))
                {
                    usersRoleUser.Add(user1);
                }
            }

            foreach (var userRoleUser in usersRoleUser)
            {
                await this._unitOfWork.Users.LoadMainAvatar(userRoleUser.UserName);
            }


            return usersRoleUser;
        }

        private async Task<bool> IsOnlyRoleUser(AppUser user)
        {
            //Only role User or not has any roles

            var roles = await this._userManager.GetRolesAsync(user);

            bool flag = true;
            foreach (var role in roles)
            {
                if (role != RoleNames.RoleUser)
                    flag = false;
            }

            return flag;
        }

        public async Task<bool> IsFollowed(string followerUsername, string followingUsername)
        {
            var follower = await this._unitOfWork.Users.GetUser(followerUsername);
            var following = await this._unitOfWork.Users.GetUser(followingUsername);

            await this._unitOfWork.Users.LoadFollowing(follower.UserName);

            return follower.Followings.Contains(following);
        }

        
    }
}
