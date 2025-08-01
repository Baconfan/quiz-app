using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuizApi.Entities;
using QuizApi.Interfaces;
using QuizApi.Models;

namespace QuizApi.Services;

public class TokenService(IConfiguration config): ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot get token Key");
        if(tokenKey.Length < 64) 
            throw new Exception("TokenKey is too short");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
        };
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDesriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDesriptor);
        
        return tokenHandler.WriteToken(token);
    }
}