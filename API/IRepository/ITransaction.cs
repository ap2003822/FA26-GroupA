using System.Transactions;
using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.Models;

namespace AllianceInventoryApi.IRepository
{
    public interface ITransaction
    {
        Task<List<TransactionModel>> GetAllTransactions();
    
    }
}
