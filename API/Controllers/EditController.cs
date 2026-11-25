using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllianceInventoryApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class EditController : Controller
    {
        private readonly IEdit repository;
        private IConfiguration config;

        public EditController(
            IEdit repo,
            IConfiguration config)
        {
            repository = repo;
            this.config = config;
        }

        [HttpGet("GetAllEdits")]
        public List<Edit> GetAllEdits()
        {
            return repository.GetAllEdits();
        }

        [HttpGet("GetEdit")]
        public async Task<Edit> GetEdit(int SId)
        {
            return await repository
                .GetEditBySId(SId);
        }

        [HttpPost("CreateEdit")]
        public async Task<bool> CreateEdit(
            Edit edit)
        {
            return await repository
                .CreateEdit(edit);
        }

        [HttpPut("UpdateEdit")]
        public async Task<bool> UpdateEdit(
            Edit edit)
        {
            return await repository
                .UpdateEdit(edit);
        }

        [HttpDelete("DeleteEdit")]
        public async Task<bool> DeleteEdit(int editId)
        {
            try
            {
                var edit =
                    await repository.GetEditBySId(editId);

                if (edit == null || edit.Item_ID == 0)
                {
                    return false;
                }

                return await repository.DeleteEdit(edit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}