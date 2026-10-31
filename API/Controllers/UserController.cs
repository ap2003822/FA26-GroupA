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
                            var tokenString = GenerateJwtToken(tokenData);

                            response.StatusCode = 200;
                            response.Status = true;
                            response.Message = "Login Successful";
                            response.AuthToken = tokenString;
                            response.User = new UserModel
                            {
                                User_id = tokenData.User_id,
                                User_Email = tokenData.User_Email,
                                User_Password = tokenData.User_Password,
                                User_Role = tokenData.User_Role
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
        private string GenerateJwtToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenAuthentication:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.User_Email),
                //new Claim(JwtRegisteredClaimNames., userInfo.Password),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            DateTime expiredDate = DateTime.UtcNow.AddDays(15);
            var token = new JwtSecurityToken(config["TokenAuthentication:Issuer"],
                config["TokenAuthentication:Issuer"],
                claims,
                expires: expiredDate,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
