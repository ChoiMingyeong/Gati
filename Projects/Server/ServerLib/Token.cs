using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class Token
    {
        private readonly JwtSecurityTokenHandler JwtTokenHandler = new JwtSecurityTokenHandler();

        // JWT 토큰 발급
        public string CreateJwtToken(string name)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, "admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key-test-12312312313"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //SecurityAlgorithms 종류 확인

            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var jwtToken = JwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
