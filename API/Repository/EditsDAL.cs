using AllianceInventoryApi.API.Data;
using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AllianceInventoryApi.API.Repository
{
    public class EditsDAL : IEdits
    {
        private readonly InventoryDbContext _context;
        private readonly IConfiguration _config;

        public EditsDAL(
            InventoryDbContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public List<Edits> GetAllEdits()
        {
            var editList = new List<Edits>();

            try
            {
                var query = _context.Edit.ToList();

                foreach (var edit in query)
                {
                    editList.Add(new Edits
                    {
                        Edit_ID = edit.Edit_ID,
                        Item_ID = edit.Item_ID,
                        User_ID = edit.User_ID,
                        Transaction_Type = edit.Transaction_Type,
                        Transaction_Date = edit.Transaction_Date,
                        Change_Desc = edit.Change_Desc,
                        Old_Value = edit.Old_Value,
                        New_Value = edit.New_Value
                    });
                }

                return editList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Edits>();
            }
        }

        public async Task<Edits> GetEditByID(int editId)
        {
            try
            {
                var query = await _context.Edit
                    .SingleOrDefaultAsync(x =>
                        x.Edit_ID == editId);

                if (query == null)
                {
                    return new Edits();
                }

                return new Edits
                {
                    Edit_ID = query.Edit_ID,
                    Item_ID = query.Item_ID,
                    User_ID = query.User_ID,
                    Transaction_Type = query.Transaction_Type,
                    Transaction_Date = query.Transaction_Date,
                    Change_Desc = query.Change_Desc,
                    Old_Value = query.Old_Value,
                    New_Value = query.New_Value
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Edits();
            }
        }

        public async Task<bool> CreateEdit(Edits edit)
        {
            try
            {
                await _context.Edit.AddAsync(edit);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateEdit(Edits edit)
        {
            try
            {
                _context.Edit.Update(edit);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteEdit(Edits edit)
        {
            try
            {
                _context.Edit.Remove(edit);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}