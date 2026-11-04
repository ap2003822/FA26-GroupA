namespace AllianceInventoryApi.API.Models
{
    public class GetAllEditResponseModel
    {
        public bool Status { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<Edit> EditList { get; set; }
            = new List<Edit>();
    }
}