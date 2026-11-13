using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data;
using AllianceInventoryApi.Models;

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
        public async Task<Item> GetItemById(int Id)
        {
            Item? RequestedItem = null;

            try
            {
                if (Id > 0)
                {
                    RequestedItem = await Task.Run(() =>
                    {
                        var query = _context.Item.SingleOrDefault(x => x.Item_ID == Id);

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

        #region Create item
        public async Task<GetItemResponseModel> CreateItem(Item item)
        {
            GetItemResponseModel response = new GetItemResponseModel();
            try
            {
                // Very basic check. Most of the verification should be done on the front end, but we should still check for nulls here.
                if (item == null)
                {
                    response.Status = false;
                    response.StatusCode = 400;
                    response.Message = "Item name is required.";
                    return response;
                }

                // Basic duplicate check.
                var name = item.Item_Name.Trim();
                var exists = await _context.Item
                    .AnyAsync(i => i.Item_Name.ToLower() == name.ToLower());
                if (exists)
                {
                    response.Status = false;
                    response.StatusCode = 409;
                    response.Message = "An item with that name already exists.";
                    return response;
                }

                // build a fresh item from the checked values
                Item newItem = new Item
                {
                    Item_Name = item.Item_Name,
                    Item_Unit_Amount = item.Item_Unit_Amount,
                    Item_Image = item.Item_Image,
                    Item_Desc = item.Item_Desc,
                    Item_Purchase_Date = item.Item_Purchase_Date,
                    Item_Low_ALert_Number = item.Item_Low_ALert_Number,
                    Item_Service_Date = item.Item_Service_Date,
                    Item_Serial_Number = item.Item_Serial_Number,
                    Item_Notes = item.Item_Notes
                };

                _context.Item.Add(newItem);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.StatusCode = 200;
                response.Message = "Item created.";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateItem --- " + ex.Message);
                response.Status = false;
                response.StatusCode = 500;
                response.Message = "Could not create the item.";
                response.Item = null;
                return response;
            }
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
