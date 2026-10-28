using Microsoft.AspNetCore.Mvc;
using Webapp.Models;
using System.Text.Json.Serialization;
using WebApp.Controllers;
using Webapp.Utility;
using Newtonsoft.Json;

namespace Webapp.Controllers
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
                    //DeserializeObject<GetAllItemsResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllItems API " + ex.Message);
            }
            return View(ResponseModel);
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