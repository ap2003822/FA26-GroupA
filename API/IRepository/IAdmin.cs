using AllianceInventoryApi.Models;

namespace AllianceInventoryApi.IRepository
{
    public interface IAdmin
    {
        public Task<GetItemResponseModel> AddItemCategory(AddCatModel UserInput);
    }
}
