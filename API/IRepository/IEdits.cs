using AllianceInventoryApi.API.Models;

namespace AllianceInventoryApi.API.IRepository
{
    public interface IEdits
    {
        List<Edits> GetAllEdits();

        Task<Edits> GetEditByID(int editId);

        Task<bool> CreateEdit(Edits edit);

        Task<bool> UpdateEdit(Edits edit);

        Task<bool> DeleteEdit(Edits edit);
    }
}