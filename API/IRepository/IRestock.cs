using AllianceInventoryApi.Data;
using AllianceInventoryApi.Models;

namespace AllianceInventoryApi.IRepository
{
    public interface IRestock
    {
        List<Item> CheckRestock();
    }
}
