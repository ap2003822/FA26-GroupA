using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.IRepository;
using AllianceInventoryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AllianceInventoryApi.Repository
{
    public class AdminDAL : IAdmin
    {
        private readonly InventoryDbContext _context;
        private readonly IConfiguration _config;
        public AdminDAL(InventoryDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        #region Add Item Cat
        public async Task<GetItemResponseModel> AddItemCategory(AddCatModel UserInput)
        {
            Item NewItemCat;

            GetItemResponseModel response = new GetItemResponseModel();

            try
            {
                NewItemCat = new Item
                { 
                    Item_Name = UserInput.Item_Name,
                    Item_Serial_Number = UserInput.Item_Serial_Number,
                    Item_Desc = UserInput.Item_Desc,
                    Item_Image = UserInput.Item_Image,
                    Item_Notes = UserInput.Item_Notes,
                    Item_Unit_Amount = UserInput.Item_Unit_Amount,
                    Item_Low_ALert_Number = UserInput.Item_Low_ALert_Number,
                    Item_Purchase_Date = UserInput.Item_Purchase_Date,
                    Item_Service_Date = UserInput.Item_Service_Date
                };

                var highestID = await _context.Item.MaxAsync(x => x.Item_ID);

                if (highestID == 0)
                {
                    highestID = 1;
                }

                NewItemCat.Item_ID = highestID + 1;

                using (var db = new InventoryDbContext(_config))
                {
                    db.Item.Add(NewItemCat);
                    await db.SaveChangesAsync();
                }

                response.Status = true;
                response.StatusCode = 200;
                response.Message = "New Category added!";
                response.Item = NewItemCat;
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddItemCategory --- " + ex.Message);
                response.Status = false;
                response.StatusCode = 0;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
        #region Remove Item Cat
        public async Task<GetItemResponseModel> RemoveItemCategory(int SerialNum)
        {
            Item RemovedItemCat;
            
            GetItemResponseModel response = new GetItemResponseModel();

            try
            {
                var query = _context.Item.SingleOrDefault(x => x.Item_Serial_Number == SerialNum);
                if (query != null) {
                    using (var db = new InventoryDbContext(_config))
                    {
                        db.Item.Remove(query);
                        await db.SaveChangesAsync();
                    }
                    response.Status = true;
                    response.StatusCode=200;
                    response.Message = "Item Removed";
                    response.Item = query;
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = 0;
                    response.Message = "Item with Serial num " + SerialNum + " Not Found";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RemoveItemCategory --- " + ex.Message);
                response.Status = false;
                response.StatusCode = 0;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
        #region Delete User
        public async Task<DeleteUserResponseModel> DeleteUser(int Id)
        {
            DeleteUserResponseModel response = new DeleteUserResponseModel();
            if (Id > 0)
            {

                try
                {
                    var query = _context.User.SingleOrDefault(x => x.User_id == Id);
                    if (query != null)
                    {
                        using (var db = new InventoryDbContext(_config))
                        {
                            db.User.Remove(query);
                            await db.SaveChangesAsync();
                        }
                        response.Status = true;
                        response.StatusCode = 200;
                        response.Message = "User Removed";
                        response.User = query;
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = 0;
                        response.Message = "No User with Id " + Id;
                        response.User = query;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DeleteUser --- " + ex);
                    response.Status = false;
                    response.StatusCode = 0;
                    response.Message = ex.Message;

                }
            }
            else
            {
                response.Status = false;
                response.StatusCode = 0;
                response.Message = "Id Empty";
            }
            return response;
        }
        #endregion
    }
}
