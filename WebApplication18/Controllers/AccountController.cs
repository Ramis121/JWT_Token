using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication18.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTSettings _jwt;
        public AccountController(IOptions<JWTSettings> jwt)
        {
            _jwt = jwt.Value;
        }

        [HttpGet("GetToken")]
        public string GetToken()
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "Ramis"));
            claims.Add(new Claim("level", "777"));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));

            var jwt = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
