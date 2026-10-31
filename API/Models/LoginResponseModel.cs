namespace AllianceInventoryApi.Models
{
    public class LoginResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AuthToken { get; set; } 
        public UserModel? User { get; set; }
    }
}
