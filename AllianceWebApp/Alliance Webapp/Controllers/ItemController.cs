using Microsoft.AspNetCore.Mvc;
using Alliance_Webapp.Models;
using Newtonsoft.Json;
using Alliance_Webapp.Utility;
using Alliance_Webapp.Pages;
using System.Text.Json.Serialization;

namespace Alliance_Webapp.Controllers
{
    public class ItemController : Controller
    {
        public IConfiguration _configuration;
        
        public ItemController(IConfiguration config)
        {
            _configuration = config;
        }
        public async Task<IActionResult> Index()
        {
            GetAllItemsResponseModel ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllItems, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetAllItemsResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllItems API " + ex.Message);
            }
            return View(ResponseModel);
        }

        [HttpGet]
        public async Task<IActionResult> ItemDetail([FromQuery(Name = "SId")]int Serial)
        { //Temp response Model, replacing with GetItem, used for this and barcode scan.
            Item ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllItems, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<Item>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetItem API" + ex.Message);
            }
            return View();
        }

        public async Task<IActionResult> GetItems()
        {
            List<GetAllItemsResponseModel> itemList = new List<GetAllItemsResponseModel>();

            GetAllItemsResponseModel? ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllItems, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetAllItemsResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllItems " + ex.Message);
            }
            return View(ResponseModel);

        }
    }
}
