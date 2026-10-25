using AllianceInventoryApi.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.Models;


namespace AllianceInventoryApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class ItemController : Controller
    {
        // private readonly IUser repository;
        private readonly IItem repository;

        //context for the database connection
        private readonly InventoryDbContext context;

        //variable for holding the configuration data for login authentication
        private IConfiguration config;

        public ItemController(IItem repo, IConfiguration config)
        {
            repository = repo;
            this.config = config;
        }


        #region Get All Items
        /// <summary>
        /// Method to return all items from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllItems", Name = "GetAllItems")]
        [AllowAnonymous]
        public async Task<GetAllItemResponseModel> GetAllItems()
        {
            GetAllItemResponseModel response = new GetAllItemResponseModel();

            //set up a list to hold the list of trips we will get back from the database
            List<Item> itemList = new List<Item>();
            try
            {
                itemList = repository.GetAllItems();

                //check the list isn't empty
                if (itemList.Count != 0)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.ItemList = itemList;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Get Failed";
                    response.StatusCode = 0;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 0;
                //there has been an error
                // Just keep during testing/building
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion

        #region Restock Email
        [HttpGet("RestockEmail", Name = "RestockEmail")]
        [AllowAnonymous]
        public async Task<RestockEmailResponseModel> RestockEmail()
        {
            RestockEmailResponseModel response = new RestockEmailResponseModel();
            List<Item> RestockList = new List<Item>();
            RestockFormat RestockEmail = new RestockFormat();
            try
            {
                RestockList = repository.RestockCheck();

                if (RestockList.Count != 0)
                {
                    RestockEmail.EmailText = RestockEmail.Beggining;
                    foreach (Item restock in RestockList)
                    {
                        RestockEmail.EmailText += $"{restock.Item_Name} | ID: {restock.Item_Serial_Number} | {restock.Item_Unit_Amount}/{restock.Item_Low_ALert_Number}\n";
                    }
                    RestockEmail.EmailText += RestockEmail.End;

                    //Find how to send email, it goes here

                    /*if (Email sends right)
                    response.Status = True;
                    response.StatusCode = 200;
                    response.Message = "Check Success, Email Sent With Restockables"
                    response.Email = RestockEmail.EmailText
                    
                     else 
                    { 
                     response.Status = False;
                     response.Status Code = 0;
                     response.Message = "Check Success, Email Send Failed."
                     }
                     */
                }
                else
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.Message = "Check Success, No Restockables found";
                    response.Email = "";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = 0;
                response.Message = "Check Failure, Unable to Send";
                response.Email = null;
            }
            return response;
        }
        #endregion
    }
}
