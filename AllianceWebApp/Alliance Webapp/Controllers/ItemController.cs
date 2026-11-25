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
        private record StockResponse(bool Status, int StatusCode, string? Message);
        private readonly IHttpClientFactory _httpClientFactory;
        private record ApiItemResponse(bool Status, int StatusCode, string? Message);
        public IConfiguration _configuration;
        public ItemController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
        // Theese three methods are used to add or subtract stock from an item. They call the ChangeStock method, which handles the actual API request and error handling.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> AddStock(int itemId, int amount) =>
            ChangeStock("Item/AddSupplyStock", "amountToAdd", itemId, amount, "Added");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> SubtractStock(int itemId, int amount) =>
            ChangeStock("Item/SubtractSupplyStock", "amountToSubtract", itemId, amount, "Removed");

        private async Task<IActionResult> ChangeStock(
            string route, string amountName, int itemId, int amount, string verb)
        {
            if (amount <= 0)
            {
                TempData["Error"] = "Enter a number greater than 0.";
                return RedirectToAction("Details", new { id = itemId });
            }

            var client = _httpClientFactory.CreateClient("Api");
            try
            {
                // the numbers go in the URL, with no request body
                var response = await client.PostAsync(
                    $"{route}?itemId={itemId}&{amountName}={amount}", null);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Stock API error: " + (int)response.StatusCode + " "
                        + await response.Content.ReadAsStringAsync());
                    TempData["Error"] = "Could not update the stock.";
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<StockResponse>();
                    if (result != null && result.Status)
                        TempData["Message"] = $"{verb} {amount}.";
                    else
                        TempData["Error"] = result?.Message ?? "Could not update the stock.";
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Could not reach the server. Please try again.";
            }

            return RedirectToAction("ItemDetail", new { id = itemId });
        }
      public IActionResult AddItem() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem([Bind("Item_Name,Item_Desc,Item_Notes,Item_Unit_Amount,Item_Low_ALert_Number,Item_Purchase_Date,Item_Service_Date,Item_Serial_Number")] Item item, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return View(item);

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!imageFile.ContentType.StartsWith("image/"))
                {
                    ViewBag.ErrorMessage = "Please choose an image file.";
                    return View(item);
                }
                if (imageFile.Length > 5 * 1024 * 1024)   // 5 MB
                {
                    ViewBag.ErrorMessage = "The image must be under 5 MB.";
                    return View(item);
                }

                using var ms = new MemoryStream();
                await imageFile.CopyToAsync(ms);
                item.Item_Image = ms.ToArray();

            }
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

