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
        private readonly IHttpClientFactory _httpClientFactory;
        private record ApiItemResponse(bool Status, int StatusCode, string? Message);
        public IConfiguration _configuration;
        public ItemController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        //public ItemController(IConfiguration config)
        //{
        //    _configuration = config;
        //}
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
        public async Task<IActionResult> ItemDetail(int id)
        {
            GetItemResponseModel ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetItemById + "?Id=" + id, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetItemResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetItem API" + ex.Message);
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
        public IActionResult AddItem() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem([Bind("Item_Name,Item_Desc,Item_Notes,Item_Unit_Amount,Item_Low_ALert_Number,Item_Purchase_Date,Item_Service_Date,Item_Serial_Number")] Item item)
        {
            ModelState.Remove(nameof(Item.Item_ID));
            if (!ModelState.IsValid) return View(item);

            var client = _httpClientFactory.CreateClient("Api");

            HttpResponseMessage message;
            try
            {
                message = await client.PostAsJsonAsync("Item/AddItem", item);   // must match the API's route
            }
            catch (HttpRequestException)
            {
                ViewBag.ErrorMessage = "Could not reach the server. Please try again.";
                return View(item);
            }

            if (!message.IsSuccessStatusCode)
            {
                Console.WriteLine("AddItem API error: " + (int)message.StatusCode + " "
                    + await message.Content.ReadAsStringAsync());
                ViewBag.ErrorMessage = "Could not create the item. Please try again.";
                return View(item);
            }

            var result = await message.Content.ReadFromJsonAsync<ApiItemResponse>();

            if (result == null || !result.Status || result.StatusCode != 200)
            {
                ViewBag.ErrorMessage = result?.Message ?? "Could not create the item.";
                return View(item);
            }

            TempData["Message"] = $"Added {item.Item_Name}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
