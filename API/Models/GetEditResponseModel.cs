namespace AllianceInventoryApi.API.Models
{
    public class GetEditResponseModel
    {
        public bool Status { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public Edit? Edit { get; set; }
    }
}