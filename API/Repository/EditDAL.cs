using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data;

namespace AllianceInventoryApi.API.Repository
{
    public class EditDAL : IEdit
    {
        private readonly InventoryDbContext _context;
        private readonly IConfiguration _config;

        public EditDAL(InventoryDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        #region Get All Edits

        public List<Edit> GetAllEdits()
        {
            var editList = new List<Edit>();

            try
            {
                var edits = _context.Edit.ToList();

                foreach (Edit edit in edits)
                {
                    editList.Add(new Edit()
                    {
                        Item_ID = edit.Item_ID,
                        Item_Desc = edit.Item_Desc,
                        Item_Name = edit.Item_Name,
                        Item_Image = edit.Item_Image,
                        Item_Notes = edit.Item_Notes,
                        Item_Unit_Amount = edit.Item_Unit_Amount,
                        Item_Serial_Number = edit.Item_Serial_Number,
                        Item_Purchase_Date = edit.Item_Purchase_Date,
                        Item_Service_Date = edit.Item_Service_Date,
                        Item_Low_ALert_Number = edit.Item_Low_ALert_Number
                    });
                }

                return editList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllEdits --- " + ex.Message);
                throw;
            }
        }

        #endregion

        #region Get Edit

        public async Task<Edit> GetEditBySId(int SId)
        {
            Edit? requestedEdit = null;

            try
            {
                if (SId > 0)
                {
                    requestedEdit = await Task.Run(() =>
                    {
                        var query = _context.Edit
                            .SingleOrDefault(x =>
                                x.Item_Serial_Number == SId);

                        if (query != null)
                        {
                            return new Edit
                            {
                                Item_ID = query.Item_ID,
                                Item_Serial_Number = query.Item_Serial_Number,
                                Item_Name = query.Item_Name,
                                Item_Desc = query.Item_Desc,
                                Item_Notes = query.Item_Notes,
                                Item_Image = query.Item_Image,
                                Item_Unit_Amount = query.Item_Unit_Amount,
                                Item_Low_ALert_Number = query.Item_Low_ALert_Number,
                                Item_Purchase_Date = query.Item_Purchase_Date,
                                Item_Service_Date = query.Item_Service_Date
                            };
                        }

                        return new Edit();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return requestedEdit ?? new Edit();
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
                    "CreateEdit --- " + ex.Message);

                return false;
            }
        }

        #endregion

        #region Update Edit

        public async Task<bool> UpdateEdit(Edit edit)
        {
            try
            {
                var existingEdit =
                    await _context.Edit
                        .FirstOrDefaultAsync(
                            x => x.Item_ID == edit.Item_ID);

                if (existingEdit == null)
                {
                    return false;
                }

                existingEdit.Item_Name = edit.Item_Name;
                existingEdit.Item_Desc = edit.Item_Desc;
                existingEdit.Item_Image = edit.Item_Image;
                existingEdit.Item_Unit_Amount = edit.Item_Unit_Amount;
                existingEdit.Item_Serial_Number = edit.Item_Serial_Number;
                existingEdit.Item_Purchase_Date = edit.Item_Purchase_Date;
                existingEdit.Item_Service_Date = edit.Item_Service_Date;
                existingEdit.Item_Notes = edit.Item_Notes;
                existingEdit.Item_Low_ALert_Number = edit.Item_Low_ALert_Number;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "UpdateEdit --- " + ex.Message);

                return false;
            }
        }

        #endregion

        #region Delete Edit

        public async Task<bool> DeleteEdit(int itemId)
        {
            try
            {
                var edit =
                    await _context.Edit
                        .FirstOrDefaultAsync(
                            x => x.Item_ID == itemId);

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
                    "DeleteEdit --- " + ex.Message);

                return false;
            }
        }

        #endregion
    }
}