using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tradeMarketPlace.Models;

namespace tradeMarketPlace.Configuration
{
    public class jwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        public jwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
         
    }
        public String GenerateJwt(string UserId, string UserEmail, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claim is used to add identity to JWT token
            var claims = new[] {
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim(JwtRegisteredClaimNames.Sid, UserId),
         new Claim(JwtRegisteredClaimNames.Email, UserEmail),
         new Claim(ClaimTypes.Role,Role),
         new Claim("Date", DateTime.Now.ToString()),
         };

            var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"],
              _configuration["JwtConfig:Audiance"],
              claims,    //null original value
              expires: DateTime.Now.AddMinutes(120),

              //notBefore:
              signingCredentials: credentials);

            string Data = new JwtSecurityTokenHandler().WriteToken(token); //return access token 
            return Data;
        }
    }
}
