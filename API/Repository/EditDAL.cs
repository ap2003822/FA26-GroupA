using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AllianceInventoryApi.API.DAL
{
    public class EditDAL : IEdit
    {
        private readonly InventoryDbContext _context;

        public EditDAL(InventoryDbContext context)
        {
            _context = context;
        }

        #region Get All Edits

        public List<Edit> GetAllEdits()
        {
            try
            {
                return _context.Edit
                    .OrderBy(x => x.ItemName)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "GetAllEdits --- " + ex.Message);

                return new List<Edit>();
            }
        }

        #endregion

        #region Get Edit By Serial Number

        public async Task<Edit> GetEditBySerialNumber(
            string serialNumber)
        {
            Edit edit = new Edit();

            try
            {
                var query =
                    await _context.Edit
                        .FirstOrDefaultAsync(
                            x => x.ItemSerialNumber ==
                                 serialNumber);

                if (query != null)
                {
                    edit = query;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "GetEditBySerialNumber --- "
                    + ex.Message);
            }

            return edit;
        }

        #endregion

        #region Create Edit

        public async Task<bool> CreateEdit(Edit edit)
        {
            try
            {
                await _context.Edit.AddAsync(edit);

                int result =
                    await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "CreateEdit --- "
                    + ex.Message);

                return false;
            }
        }

        #endregion

        #region Update Edit

        public async Task<bool> UpdateEdit(Edit edit)
        {
            try
            {
                var existing =
                    await _context.Edit
                        .FirstOrDefaultAsync(
                            x => x.ItemSerialNumber ==
                                 edit.ItemSerialNumber);

                if (existing == null)
                {
                    return false;
                }

                existing.ItemName =
                    edit.ItemName;

                existing.ItemUnitAmount =
                    edit.ItemUnitAmount;

                existing.ItemImage =
                    edit.ItemImage;

                existing.ItemDesc =
                    edit.ItemDesc;

                existing.ItemPurchaseDate =
                    edit.ItemPurchaseDate;

                existing.ItemServiceDate =
                    edit.ItemServiceDate;

                existing.ItemNotes =
                    edit.ItemNotes;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "UpdateEdit --- "
                    + ex.Message);

                return false;
            }
        }

        #endregion

        #region Delete Edit

        public async Task<bool> DeleteEdit(
            string serialNumber)
        {
            try
            {
                var edit =
                    await _context.Edit
                        .FirstOrDefaultAsync(
                            x => x.ItemSerialNumber ==
                                 serialNumber);

                if (edit == null)
                {
                    return false;
                }

                _context.Edit.Remove(edit);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "DeleteEdit --- "
                    + ex.Message);

                return false;
            }
        }

        #endregion
    }
}