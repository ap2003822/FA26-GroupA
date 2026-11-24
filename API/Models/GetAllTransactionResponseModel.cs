using AllianceInventoryApi.API.Models;

namespace AllianceInventoryApi.Models
{
    public class GetAllTransactionResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}
