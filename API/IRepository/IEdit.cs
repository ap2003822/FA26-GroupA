using AllianceInventoryApi.API.Models;

namespace AllianceInventoryApi.API.IRepository
{
    public interface IEdit
    {
        List<Edit> GetAllEdits();

        Task<Edit> GetEditBySerialNumber(string serialNumber);

        Task<bool> CreateEdit(Edit edit);
    }
}