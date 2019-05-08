using Apotheca.BLL.Models;
using Apotheca.BLL.Repositories;
using Apotheca.BLL.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Apotheca.BLL.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly string _appSecret;
        private readonly IUserRepository _userRepo;
        private readonly IPasswordProvider _passwordProvider;

        //private readonly AppSettings _appSettings;

        public AuthService(string appSecret, IUserRepository userRepo, IPasswordProvider passwordProvider)//IOptions<AppSettings> appSettings)
        {
            this._appSecret = appSecret;
            this._userRepo = userRepo;
            this._passwordProvider = passwordProvider;
            //_appSettings = appSettings.Value;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _userRepo.GetByEmail(email);

            // return null if user not found
            if (user == null)
                return null;

            // check that the password is correct
            var hashedPassword = _passwordProvider.HashPassword(password, user.Salt);
            if (hashedPassword != user.Password)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim("MyRole", "")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

    }
}
