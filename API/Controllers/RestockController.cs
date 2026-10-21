using AllianceInventoryApi.Data;
using AllianceInventoryApi.IRepository;
using AllianceInventoryApi.Models;
using AllianceInventoryApi.Repository;
using AllianceInventoryAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllianceInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestockController : ControllerBase
    {
        private readonly IRestock repository;
        private readonly InventoryDbContext context;

        private IConfiguration config;

        public RestockController(IConfiguration Config)
        {
            config = Config;
            context = new InventoryDbContext(config);
            repository = new RestockDAL(context, config);
        }

        [HttpGet("CheckRestock", Name = "CheckRestock")]
        [AllowAnonymous]
        public async Task<RestocksResponseModel> CheckRestock()
        {
            RestocksResponseModel response = new RestocksResponseModel();

            List<Item> RestockList = new List<Item>();
            try
            {
                RestockList = await Task.Run(() => repository.CheckRestock());

                if (RestockList.Count != 0)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.Items = RestockList;
                }
                else
                {
                    //error flag
                    response.Status = false;
                    response.StatusCode = 0;
                    response.StatusMessage = "Get Failed";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = 0;
                response.StatusMessage = "Get Failed";
                Console.WriteLine(ex.Message);
            }
            return response;
        }
    }
}
