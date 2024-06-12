using AutoMapper;
using Backend.Common.CommonFunctions;
using Backend.Common.Models;
using Backend.Domain.BackendSetup;
using Backend.Dto.BackendSetup;
using Backend.Services.RepositoryConfig;
using Backend.WebHost.HostModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TestCode;
using static Backend.Common.CommonFunctions.CommonEnums;

namespace Backend.WebHost.Controllers.TaskSetup
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly IRepositoryWrapperService _manager;
        private readonly IMapper _mapper;


        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger, IRepositoryWrapperService manager, IMapper mapper)
        {
            _logger = logger;
            _manager = manager;
            _manager.CheckArgumentIsNull(nameof(manager));
            _mapper = mapper;
            _manager.CheckArgumentIsNull(nameof(mapper));
        }

        [HttpGet("GetAllTask")]
        public async Task<IActionResult> GetAllTask()
        {

            var model = _manager.TaskServices.GetAlDetails();
            return Ok(model);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long Id)
        {

            var model = _manager.TaskServices.GetDetailsById(Id);
            return Ok(model);
        }
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ActionName("Post")]
        public IActionResult Post([FromBody] BackendTaskPostDto _postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postMappedObj = _mapper.Map<BackendTask>(_postModel);
            var entityModel = _manager.TaskServices.CreateEntity(postMappedObj);
            _manager.Save();

            

           
            return Ok(CommonFunction.Response(ResponseType.SuccessId, entityModel.Id.ToString()));
        }

        [IgnoreAntiforgeryToken]
        [HttpPut("[action]")]
        [ActionName("Put")]
        public IActionResult Put([FromBody] BackendTaskPutDto _putModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _getDetailsObj = _manager.TaskServices.GetDetailsById(_putModel.Id);

            var postMappedObj = _mapper.Map<BackendTask>(_putModel);
            if (_getDetailsObj != null)
            {
                var mapObject = BackendTaskHostModel.AssignToUpdateModel(_getDetailsObj, _putModel);
                mapObject = BackendTaskHostModel.UpdatedTracking(mapObject, 1);
                _manager.TaskServices.Update(mapObject);
                _manager.Save();
            }
            return Ok(CommonFunction.Response(ResponseType.SuccessId, _getDetailsObj.Id.ToString()));
        }


        [IgnoreAntiforgeryToken]
        [HttpDelete("[action]")]
        public IActionResult Delete(long Id)
        {
            
            var _getDetailsObj = _manager.TaskServices.GetDetailsById(Id);
            if (_getDetailsObj != null )
            {
                _manager.TaskServices.Delete(_getDetailsObj);
                _manager.Save();
                return Ok(CommonFunction.Response(ResponseType.Success, ""));
            }
            else
            {
                return NotFound(CommonFunction.Response(ResponseType.Failure, "record not Delete"));
            }
        }

    }
}
