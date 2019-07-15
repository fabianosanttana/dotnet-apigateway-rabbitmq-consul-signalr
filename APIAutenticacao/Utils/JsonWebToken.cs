using ApiAutenticacao.Models;
using APIAutenticacao.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIAutenticacao.Utils
{
    public class JsonWebToken
    {
        public static string GenerateToken(LoginView user, IOptions<Audience> settings) {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Value.Chave));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = settings.Value.Iss,
                ValidateAudience = true,
                ValidAudience = settings.Value.Author,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };

            var jwt = new JwtSecurityToken(
                issuer: settings.Value.Iss,
                audience: settings.Value.Author,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            //var responseJson = new
            //{
            //    access_token = encodedJwt,
            //    expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
            //};
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
