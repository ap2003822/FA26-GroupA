namespace Alliance_Webapp.Models
{
    public class GetAllTransactionResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<Transaction> Transactions{ get; set; } = new List<Transaction>();
    }
}
