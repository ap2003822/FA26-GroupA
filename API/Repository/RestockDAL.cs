using AllianceInventoryApi.Data;
using AllianceInventoryApi.IRepository;
using AllianceInventoryAPI.Data;

namespace AllianceInventoryApi.Repository
{
    public class RestockDAL : IRestock
    {
        private readonly InventoryDbContext context;
        private readonly IConfiguration _config;
        public RestockDAL(InventoryDbContext Context, IConfiguration config)
        {
            context = Context;
            _config = config;
        }

        public List<Item> CheckRestock()
        {
            List<Item> RestockList = new List<Item>();

            try
            {
                var Items = context.Items.ToList();

                if (Items != null)
                {
                    foreach (var item in Items)
                    {
                        RestockList.Add(new Item()
                        {
                            Item_ID = item.Item_ID,
                            Item_Name = item.Item_Name,
                            Item_Unit_Amount = item.Item_Unit_Amount,
                            Item_Desc = item.Item_Desc,
                            Item_Purchase_Date = item.Item_Purchase_Date,
                            Item_Service_Date = item.Item_Service_Date,
                            Item_Low_Alert_Number = item.Item_Low_Alert_Number,
                            Item_Notes = item.Item_Notes,
                            Item_Serial_Number = item.Item_Serial_Number
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No Items in Inventory");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CheckRestock ---" + ex.Message);
                throw;
            }

            return RestockList;
        }
    }
}
