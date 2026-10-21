using AllianceInventoryApi.Models;

namespace AllianceInventoryApi.IRepository
{
    public interface IItem
    {
        /// <summary>
        /// Returns a list of all items in the database
        /// </summary>
        /// <returns></returns>
        List<Item> GetAllItems();
    }
}
