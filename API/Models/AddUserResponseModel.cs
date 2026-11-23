namespace AllianceInventoryApi.Models
{
    public class AddUserResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserModel? User { get; set; }
    }
}
