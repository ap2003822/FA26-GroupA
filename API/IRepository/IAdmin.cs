using AllianceInventoryApi.Models;

namespace AllianceInventoryApi.IRepository
{
    public interface IAdmin
    {
        public Task<GetItemResponseModel> AddItemCategory(AddCatModel UserInput);
        public Task<GetItemResponseModel> RemoveItemCategory(int SerialNum);
        public Task<DeleteUserResponseModel> DeleteUser(string Email);
    }
}
