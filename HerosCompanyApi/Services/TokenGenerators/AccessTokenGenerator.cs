using HerosCompanyApi.Models;
using HerosCompanyApi.Models.DTOs;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HerosCompanyApi.Services.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfigurations _configurations;

        public AccessTokenGenerator(AuthenticationConfigurations configurations)
        {
            _configurations = configurations;
        }

        public string GenerateToken(TrainerDTO trainer)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurations.AccessTokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", trainer.TrainerId.ToString()),
                new Claim(ClaimTypes.Name, trainer.TrainerUserName)
            };
            JwtSecurityToken token = new JwtSecurityToken(
                _configurations.Issuer,
                _configurations.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_configurations.AccessTokenExpirationMinutes),
                credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TrainerDTO ReadToken(ClaimsPrincipal principal)
        {
            Guid id = Guid.Parse(principal.Claims.Where(c => c.Type == "id")
                                                 .FirstOrDefault().Value);
            return new TrainerDTO(id, principal.Identity.Name);
        }
    }
}
