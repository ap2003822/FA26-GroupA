using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.Models;

namespace AllianceInventoryApi.IRepository
{
    public interface IUser
    {
        Task<LoginResponseModel> LoginUser(UserModel tokenData);
        Task<UserModel> FindUserByEmail(string email);
    }
}
