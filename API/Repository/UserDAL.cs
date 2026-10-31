using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.IRepository;
using AllianceInventoryApi.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AllianceInventoryApi.Repository
{
    public class UserDAL : IUser
    {
        private readonly InventoryDbContext _context;
        private readonly IConfiguration _config;
        public UserDAL(InventoryDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        #region Login User
        public async Task<LoginResponseModel> LoginUser(UserModel tokenData)
        {
            LoginResponseModel response = new LoginResponseModel();
            try
            {
                if(tokenData != null)
                {
                    var query = _context.User
                        .Where(x => x.User_Email == tokenData.User_Email && x.User_Password == tokenData.User_Password)
                        .FirstOrDefault<UserModel>();
                    if(query != null)
                    {
                        response.Status = true;
                        response.StatusCode = 200;
                        response.Message = "Login Success";
                        response.User = query;
                        return response;
                    }
                    else
                    {
                        response.Status= false;
                        response.StatusCode = 500;
                        response.Message = "Login Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginUser --- " + ex.Message);
                response.Status = false;
                response.StatusCode = 500;
            }
            return response;
        }
        #endregion
        #region FindUserByEmail
        public async Task<UserModel> FindUserByEmail(string email)
        {
            UserModel? user = null;

            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    user = await Task.Run(() =>
                    {
                        var query = _context.User
                        .Where(x => x.User_Email == email)
                        .FirstOrDefault<UserModel>();

                        if (query != null)
                        {
                            return new UserModel
                            {
                                User_id = query.User_id,
                                User_Email = query.User_Email,
                                User_Password = query.User_Password,
                                User_Role = query.User_Role
                            };
                        }
                        return null;
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindUserByEmail --- " + ex.Message);
            }

            return user ?? new UserModel();
        }
        #endregion
    }
}
