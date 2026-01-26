using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MovieDB.SharedModels;
using System.Security.Claims;
using System.Text;

namespace MovieDB.REST_API.Services
{
    public class TokenProviderService(IConfiguration configuration)
    {
        public string Create(User user)
        {
            SymmetricSecurityKey authenticationKey = new(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));

            SigningCredentials credentials = new(authenticationKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                ([
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
                ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JWT:Expiration")),
                SigningCredentials = credentials,
                Issuer = configuration["JWT:Issuer"],
                Audience = configuration["JWT:Audience"]
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
    }
}
