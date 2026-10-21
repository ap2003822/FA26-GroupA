using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AllianceInventoryApi.API.Repository
{
    public class ItemDAL : IItem
    {
        private readonly InventoryDbContext _context;

        public ItemDAL(InventoryDbContext context)
        {
            _context = context;
        }
        #region GetAllItem
        public List<Item> GetAllItems()
        {
            var itemList = new List<Item>();

            try
            {
                //query the database to get all of the trips
                var items = _context.Item.ToList();

                foreach (Item item in items)
                {
                    //get all of the object data to send back
                    itemList.Add(new Item()
                    {
                        Item_ID = item.Item_ID,
                        Item_Desc = item.Item_Desc,
                        Item_Name = item.Item_Name,
                        Item_Low_ALert_Number = item.Item_Low_ALert_Number,
                        Item_Notes = item.Item_Notes,
                        Item_Purchase_Date = item.Item_Purchase_Date,
                        Item_Serial_Number = item.Item_Serial_Number,
                        Item_Service_Date = item.Item_Service_Date,
                        Item_Unit_Amount = item.Item_Unit_Amount,
                    });
                }

                return itemList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllItems --- " + ex.Message);
                throw;
            }
        }
        #endregion

    }
}
