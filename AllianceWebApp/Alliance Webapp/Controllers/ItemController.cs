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

    }
}
