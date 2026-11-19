using AllianceInventoryApi.API.Models;

namespace AllianceInventoryApi.API.IRepository
{
    public interface IEdit
    {
        List<Edit> GetAllEdits();

        Task<Edit> GetEditBySId(int SId);

        Task<bool> CreateEdit(Edit edit);

        Task<bool> UpdateEdit(Edit edit);

        Task<bool> DeleteEdit(Edit edit);
    }
}