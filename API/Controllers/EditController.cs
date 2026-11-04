using AllianceInventoryApi.API.Models;
using AllianceInventoryApi.API.IRepository;
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

        public EditController(IEdit repo, IConfiguration config)
        {
            repository = repo;
            this.config = config;
        }

        #region Get All Edits

        [HttpGet("GetAllEdits", Name = "GetAllEdits")]
        [AllowAnonymous]
        public async Task<GetAllEditResponseModel> GetAllEdits()
        {
            GetAllEditResponseModel response =
                new GetAllEditResponseModel();

            try
            {
                List<Edit> editList =
                    repository.GetAllEdits();

                if (editList.Count > 0)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.EditList = editList;
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = 0;
                    response.Message = "No Edits Found";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = 0;
                response.Message = ex.Message;
            }

            return response;
        }

        #endregion

        #region Get Edit

        [HttpGet("GetEdit")]
        [AllowAnonymous]
        public async Task<GetEditResponseModel> GetEdit(string serialNumber)
        {
            GetEditResponseModel response =
                new GetEditResponseModel();

            try
            {
                Edit edit =
                    await repository.GetEditBySerialNumber(serialNumber);

                if (edit != null)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.Edit = edit;
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = 0;
                    response.Message = "Edit Not Found";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = 0;
                response.Message = ex.Message;
            }

            return response;
        }

        #endregion

        #region Create Edit

        [HttpPost("CreateEdit")]
        [AllowAnonymous]
        public async Task<CreateEditResponseModel> CreateEdit(Edit edit)
        {
            CreateEditResponseModel response =
                new CreateEditResponseModel();

            try
            {
                bool result =
                    await repository.CreateEdit(edit);

                if (result)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.Message = "Edit Created Successfully";
                    response.Edit = edit;
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = 0;
                    response.Message = "Create Failed";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = 0;
                response.Message = ex.Message;
            }

            return response;
        }

        #endregion
    }
}