using AllianceInventoryApi.Data;

namespace AllianceInventoryApi.Models
{
    public class RestocksResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string? StatusMessage { get; set; } = null;
        public List<Item>? Items { get; set; } = null;
    }
}
