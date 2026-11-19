using AllianceInventoryApi.API.IRepository;
using AllianceInventoryApi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllianceInventoryApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class EditsController : Controller
    {
        private readonly IEdits repository;
        private IConfiguration config;

        public EditsController(
            IEdits repo,
            IConfiguration config)
        {
            repository = repo;
            this.config = config;
        }

        [HttpGet("GetAllEdits")]
        public List<Edits> GetAllEdits()
        {
            return repository.GetAllEdits();
        }

        [HttpGet("GetEdit")]
        public async Task<Edits> GetEdit(int editId)
        {
            return await repository.GetEditByID(editId);
        }

        [HttpPost("CreateEdit")]
        public async Task<bool> CreateEdit(Edits edit)
        {
            return await repository.CreateEdit(edit);
        }

        [HttpPut("UpdateEdit")]
        public async Task<bool> UpdateEdit(Edits edit)
        {
            return await repository.UpdateEdit(edit);
        }

        [HttpDelete("DeleteEdit")]
        public async Task<bool> DeleteEdit(int editId)
        {
            var edit =
                await repository.GetEditByID(editId);

            if (edit.Edit_ID == 0)
            {
                return false;
            }

            return await repository.DeleteEdit(edit);
        }
    }
}