using System.Transactions;
using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.IRepository;
using AllianceInventoryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllianceInventoryApi.Controllers
{
    // I made this its own controller because only admin users should be able to view transactions and userController allows anonymous access to the endpoints.
    [ApiController]
    [Route("[controller]")]
    public class TransactionController
    {
        private readonly ITransaction repository;
        private readonly InventoryDbContext context;
        private IConfiguration config;

        public TransactionController(ITransaction repo, IConfiguration config)
        {
            repository = repo;
            this.config = config;
        }
        #region Get All Transactions
        [HttpGet("GetAllTransactions", Name = "GetAllTransactions")]
        [AllowAnonymous] // This should be changed to [Authorize(Roles = "Admin")] after testing
        public async Task<GetAllTransactionResponseModel> GetAllTransactions()
        {
            GetAllTransactionResponseModel response = new GetAllTransactionResponseModel();

            //set up a list to hold the list of trips we will get back from the database
            List<TransactionModel> transactionList = new List<TransactionModel>();
            try
            {
                transactionList = await repository.GetAllTransactions();

                //check the list isn't empty
                if (transactionList.Count != 0)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.Transactions = transactionList;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Get Failed";
                    response.StatusCode = 0;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 0;
                //there has been an error
                // Just keep during testing/building
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion
        //#region Add Transaction To Database
        //[HttpPost("AddTransaction", Name = "AddTransaction")]
        //[AllowAnonymous] // This should be changed to [Authorize(Roles = "Admin")] after
        //public async Task<ChangeStockResponseModel> AddTransaction(TransactionModel transaction)
        //{
        //    ChangeStockResponseModel response = new ChangeStockResponseModel();
        //    try
        //    {
        //        response = await repository.ChangeStockNumber(transaction.User_Email, transaction.Item_Name, transaction.);
        //        if (response.StatusCode == 200)
        //        {
        //            response.Status = true;
        //            response.Message = "Transaction Added Successfully";
        //        }
        //        else
        //        {
        //            response.Status = false;
        //            response.Message = "Failed to add transaction";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = false;
        //        response.Message = "Failed to add transaction";
        //        response.StatusCode = 0;
        //        Console.WriteLine(ex.Message);
        //    }
        //    return response;
        //}
        //#endregion
    }
}
