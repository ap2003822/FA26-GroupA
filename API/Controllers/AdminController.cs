using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.IRepository;
using AllianceInventoryApi.Models;
using AllianceInventoryApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllianceInventoryApi.Controllers
{
    public class AdminController : Controller
    { //For Admin exclusive tasks
        private readonly IAdmin repository;

        //context for the database connection
        private readonly InventoryDbContext context;

        //variable for holding the configuration data for login authentication
        private IConfiguration config;

        public AdminController(IConfiguration Config)
        {
            config = Config;
            context = new InventoryDbContext(config);
            repository = new AdminDAL(context, config);
        }

        [HttpPost("AddItemCategory", Name = "AddItemCategory")]
        [AllowAnonymous]
        public async Task<GetItemResponseModel> AddItemCategory(AddCatModel UserInput)
        {
            GetItemResponseModel response = new GetItemResponseModel();

            if (UserInput != null)
            {
                try
                {
                    var ItemCat = await repository.AddItemCategory(UserInput).ConfigureAwait(true);

                    if(ItemCat != null && ItemCat.Item != null && ItemCat.StatusCode == 200)
                    {
                        response.Status = true;
                        response.Message = "Category Added";
                        response.StatusCode = 200;

                        response.Item = new API.Models.Item
                        {
                            Item_ID = ItemCat.Item.Item_ID,
                            Item_Name = ItemCat.Item.Item_Name,
                            Item_Serial_Number = ItemCat.Item.Item_Serial_Number,
                            Item_Desc = ItemCat.Item.Item_Desc,
                            Item_Image = ItemCat.Item.Item_Image,
                            Item_Notes = ItemCat.Item.Item_Notes,
                            Item_Unit_Amount = ItemCat.Item.Item_Unit_Amount,
                            Item_Low_ALert_Number = ItemCat.Item.Item_Low_ALert_Number,
                            Item_Purchase_Date = ItemCat.Item.Item_Purchase_Date,
                            Item_Service_Date = ItemCat.Item.Item_Service_Date
                        };
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = 0;
                        response.Message = "Category Add Failed";
                    }
                }
                catch (Exception ex)
                {
                    response.Status = false;
                    response.StatusCode = 0;
                    response.Message = ex.Message;
                    Console.WriteLine("AddItemCategory --- " + ex.Message);
                }
            }
            return response;
        }
        [HttpPost("RemoveItemCategory", Name = "RemoveItemCategory")]
        [AllowAnonymous]
        public async Task<GetItemResponseModel> RemoveItemCategory(int SerialNum)
        {
            GetItemResponseModel response = new GetItemResponseModel();

            if (SerialNum > 0)
            {
                try
                {
                    var ItemCat = await repository.RemoveItemCategory(SerialNum).ConfigureAwait(true);

                    if (ItemCat.Status == true && ItemCat.StatusCode == 200 && ItemCat.Item != null)
                    {
                        response.Status = true;
                        response.StatusCode = 200;
                        response.Message = "Item Removed";
                        response.Item = new API.Models.Item
                        {
                            Item_ID = ItemCat.Item.Item_ID,
                            Item_Name = ItemCat.Item.Item_Name,
                            Item_Serial_Number = ItemCat.Item.Item_Serial_Number,
                            Item_Desc = ItemCat.Item.Item_Desc,
                            Item_Image = ItemCat.Item.Item_Image,
                            Item_Notes = ItemCat.Item.Item_Notes,
                            Item_Unit_Amount = ItemCat.Item.Item_Unit_Amount,
                            Item_Low_ALert_Number = ItemCat.Item.Item_Low_ALert_Number,
                            Item_Purchase_Date = ItemCat.Item.Item_Purchase_Date,
                            Item_Service_Date = ItemCat.Item.Item_Service_Date
                        };
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = 0;
                        response.Message = "RemoveFalse";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("RemoveItemCategory --- " + ex.Message);
                    response.Status = false;
                    response.StatusCode = 0;
                    response.Message = ex.Message;
                }
            }
            else
            {
                response.Status = false;
                response.StatusCode = 0;
                response.Message = "No Serial ID Entered";
            }
            return response;
        }
    }
}
