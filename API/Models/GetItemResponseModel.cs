using AllianceInventoryApi.API.Models;

namespace AllianceInventoryApi.Models
{
    public class GetItemResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public Item? Item { get; set; }
    }
}
