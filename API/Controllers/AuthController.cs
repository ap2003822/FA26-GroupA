using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AllianceInventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly InventoryDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(InventoryDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public record LoginRequest(string Email, string Password);

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseModel>> Login(LoginRequest request)
    {
        var email = request.Email.Trim().ToLower();
        var user = await _context.User
            .FirstOrDefaultAsync(u => u.User_Email.ToLower() == email);

        if (user == null || !PasswordMatches(user.User_Password, request.Password))
            return Unauthorized("Invalid email or password.");

        // Collapse the many roles in the data down to two
        var role = string.Equals(user.User_Role?.Trim(), "Admin", StringComparison.OrdinalIgnoreCase)
            ? "Admin" : "User";

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.User_id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.User_Email),
            new("role", role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new LoginResponseModel
        {
            Status = true,
            StatusCode = 200,
            Message = "Login successful.",
            AuthToken = new JwtSecurityTokenHandler().WriteToken(token),
            User = new UserModel
            {
                User_id = user.User_id,
                User_Email = user.User_Email,
                User_Password = user.User_Password,
                User_Role = role
            },

        };

        

    }

    private static bool PasswordMatches(string? stored, string supplied) =>
        CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(stored ?? ""),
            Encoding.UTF8.GetBytes(supplied ?? ""));
}