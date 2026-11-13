using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.Models;

namespace AllianceInventoryApi.API.IRepository
{
    public interface IItem
    {
        /// <summary>
        /// Returns a list of all items in the database
        /// </summary>
        /// <returns></returns>
        List<Item> GetAllItems();
        List<Item> RestockCheck();
        Task<Item> GetItemById(int Id);
        Task<GetItemResponseModel>CreateItem(Item item);
    }
}
