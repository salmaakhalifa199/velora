using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using velora.core.Entities.IdentityEntities;

namespace velora.services.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<Person> _userManager;
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _Key;

        public TokenService(UserManager<Person> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public async Task<string> GenerateTokenAsync(Person user)
        {
            var authClaims = new List<Claim>
            {
              new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.GivenName, user.FirstName),
              new Claim("UserName" , user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(authClaims),
                Issuer = _config["Token:Issuer"],
                Audience = _config["Token:Audience"],
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
