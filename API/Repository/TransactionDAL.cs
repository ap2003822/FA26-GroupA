using System.Data;
using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.IRepository;
using AllianceInventoryApi.Models;
using Azure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AllianceInventoryApi.Repository
{
    public class TransactionDAL : ITransaction
    {
        private readonly InventoryDbContext _context;
        private readonly IConfiguration _config;
        public TransactionDAL(InventoryDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        #region GetAllTransaction
        public async Task<List<TransactionModel>> GetAllTransactions()
        {
            try
            {
                // grabs the 500 most recent transactions from the database and orders them by descending transaction id
                var list = await _context.Edits
                    .AsNoTracking()
                    .OrderByDescending(t => t.Transaction_Id)
                    .Take(500)
                    .ToListAsync();

                // grabs the user emails for the transactions and adds them to the transaction model
                var userIds = list.Select(t => t.User_Id).Distinct().ToList();
                var itemIds = list.Select(t => t.Item_Id).Distinct().ToList();
                var emails = await _context.User
                    .Where(u => userIds.Contains(u.User_id))
                    .ToDictionaryAsync(u => u.User_id, u => u.User_Email);
                var itemNames = await _context.Item
                    .Where(i => itemIds.Contains(i.Item_ID))
                    .ToDictionaryAsync(i => i.Item_ID, i => i.Item_Name);

                foreach (var t in list)
                    t.User_Email = emails.TryGetValue(t.User_Id, out var email) ? email : "User Not Found";
                foreach (var t in list)
                    t.Item_Name = itemNames.TryGetValue(t.Item_Id, out var itemName) ? itemName : "Item Not Found";

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllTransactions --- " + ex.Message);
                return new List<TransactionModel>();
            }
        }
        #endregion
    }
}
