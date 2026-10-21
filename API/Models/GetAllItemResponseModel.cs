namespace AllianceInventoryApi.API.Models
{
    public class GetAllItemResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<Item> ItemList { get; set; } = new List<Item>();
    }
}
