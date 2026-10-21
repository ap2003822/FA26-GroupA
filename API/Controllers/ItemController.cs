using AllianceInventoryApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;


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
    }
}
