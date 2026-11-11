using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.IRepository;
using AllianceInventoryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AllianceInventoryApi.Repository;

namespace AllianceInventoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        // private readonly IUser repository;
        private readonly IUser repository;

        //context for the database connection
        private readonly InventoryDbContext context;

        //variable for holding the configuration data for login authentication
        private IConfiguration config;

        public UserController(IConfiguration Config)
        {
            config = Config;
            context = new InventoryDbContext(config);
            repository = new UserDAL(context, config);
        }

        [HttpPost("LoginUser", Name = "LoginUser")]
        [AllowAnonymous]
        public async Task<LoginResponseModel> LoginUser(UserModel tokenData)
        {
            LoginResponseModel response = new LoginResponseModel();
            try
            {
                if (tokenData != null && !string.IsNullOrWhiteSpace(tokenData.User_Email))
                {
                    var loginResult = await repository.LoginUser(tokenData).ConfigureAwait(true);

                    if (loginResult.StatusCode == 200)
                    {
                        var user = await repository.FindUserByEmail(tokenData.User_Email).ConfigureAwait(true);
                        if(user != null)
                        {
                            var tokenString = GenerateJwtToken(user);

                            response.StatusCode = 200;
                            response.Status = true;
                            response.Message = "Login Successful";
                            response.AuthToken = tokenString;
                            response.User = new UserModel
                            {
                                User_id = user.User_id,
                                User_Email = user.User_Email,
                                User_Role = user.User_Role
                            };

                            return response;
                        }

                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Status = false;
                        response.Message = "Login Failed";
                    }
                }
                else
                {
                    response.StatusCode = 400;
                    response.Status = false;
                    response.Message = "Invalid Username";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginUser --- " + ex.Message);
                response.StatusCode = 500;
                response.Status = false;
                response.Message = "Login Failed";
            }

            return response;
        }
        private string GenerateJwtToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // collapse the many roles in the data down to two
            var role = string.Equals(user.User_Role?.Trim(), "Admin", StringComparison.OrdinalIgnoreCase)
                ? "Admin" : "User";

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.User_id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.User_Email),
        new Claim("role", role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
