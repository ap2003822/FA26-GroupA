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

        public IConfiguration _configuration;
        public ItemController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
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
        public IActionResult AddItem()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(Item item)
        {
            var client = _httpClientFactory.CreateClient("Api");

            GetItemResponseModel? result;
            try
            {
                // Send the item to the API
                var httpResponse = await client.PostAsJsonAsync("api/items", item);

                // Read the API's response model (works for success AND error responses)
                result = await httpResponse.Content.ReadFromJsonAsync<GetItemResponseModel>();
            }
            catch
            {
                ViewBag.Error = "Could not reach the server. Please try again.";
                return View(item);
            }

            if (result != null && result.Status)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction("AddItem");
            }

            // Failed: show the message and keep what the user typed
            ViewBag.Error = result?.Message ?? "Something went wrong.";
            return View(item);
        }
        public IActionResult CreateItem(Item item)
        {
            if (ModelState.IsValid)
            {
                
            }
            return View("AddItem", item);
        }
    }
}
