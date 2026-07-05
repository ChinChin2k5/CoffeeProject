/*using Microsoft.EntityFrameworkCore;
using CoffeeShop.BLL.DTOs.Inventory.Requests;
using CoffeeShop.BLL.DTOs.Inventory.Responses;
using CoffeeShop.DAL;
using CoffeeShop.Models.Entities.Auth;
using System;
using System.Text;                          // BỔ SUNG: Để dùng Encoding
using System.Security.Claims;               // BỔ SUNG: Để dùng Claim, ClaimTypes
using Microsoft.IdentityModel.Tokens;       // BỔ SUNG: Để dùng SymmetricSecurityKey, SigningCredentials
using System.IdentityModel.Tokens.Jwt;      // BỔ SUNG: Để dùng JwtSecurityTokenHandler, JwtRegisteredClaimNames
using Microsoft.Extensions.Configuration;   // BỔ SUNG: Để dùng IConfiguration
namespace CoffeeShop.BLL.TokenService 
{
    public class TokenService
    {
        private string GenerateJwtToken(string email, string role)
    {
        // Đọc cấu hình từ file appsettings.json
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        // Dùng đúng chìa khóa đó để đúc vé
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(ClaimTypes.Role, role), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,       
            audience: audience,   
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    }
}*/