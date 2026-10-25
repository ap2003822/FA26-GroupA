using AllianceInventoryApi.API.Models;

namespace AllianceInventoryApi.Models
{
    public class RestockEmailResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<Item>? RestockList { get; set; }
        public string? Email;
    }
}