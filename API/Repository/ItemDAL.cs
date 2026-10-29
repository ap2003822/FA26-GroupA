using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data;

namespace AllianceInventoryApi.API.Repository
{
    public class ItemDAL : IItem
    {
        private readonly InventoryDbContext _context;

        private readonly IConfiguration _config;

        public ItemDAL(InventoryDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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
        #region Get Item
        public async Task<Item> GetItemBySId(int SId)
        {
            Item? RequestedItem = null;

            try
            {
                if (SId > 0)
                {
                    RequestedItem = await Task.Run(() =>
                    {
                        var query = _context.Item.SingleOrDefault(x => x.Item_Serial_Number == SId);

                        if (query != null)
                        {
                            return new Item
                            {
                                Item_ID = query.Item_ID,
                                Item_Serial_Number = query.Item_Serial_Number,
                                Item_Name = query.Item_Name,
                                Item_Desc = query.Item_Desc,
                                Item_Notes = query.Item_Notes,
                                Item_Image = query.Item_Image,
                                Item_Unit_Amount = query.Item_Unit_Amount,
                                Item_Low_ALert_Number = query.Item_Low_ALert_Number,
                                Item_Purchase_Date = query.Item_Purchase_Date,
                                Item_Service_Date = query.Item_Purchase_Date
                            };
                        }
                        return new Item();
                    });
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RequestedItem ?? new Item();
        }
        #endregion
        #region Restock Email
        public List<Item> RestockCheck()
        {
            List<Item>? itemList = GetAllItems();
            List<Item>? RestockList = new List<Item>();
            try
            {
                foreach (Item item in itemList)
                {
                    if (item.Item_Unit_Amount <= item.Item_Low_ALert_Number)
                    {
                        RestockList.Add(item);
                    }
                }
                return RestockList;
            }
            catch (Exception ex)
            { //Should Only fail if GetAll Items does first, but good to call it out anyway.
                Console.Write("RestockCheck --- " + ex.Message);
                RestockList = null;
                throw;
            }
        }
        #endregion
    }
}
