using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_mangement_System.Models.Dto;
using Task_mangement_System.Models;
using Task_mangement_System.Repository.IRepository;
using Task = Task_mangement_System.Models.Task;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net;
using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace Task_mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="User,admin")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _task;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;
        public TaskController(ITaskRepository task,IMapper mapper)
        {
            _task = task;
            _mapper = mapper;
            this._apiResponse = new APIResponse();
        }
        [HttpGet]
        [ResponseCache(CacheProfileName = "default60")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllTasks()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                IEnumerable<Task> tasks = await _task.GetAll(t => t.UserId == userId, "Category");
                _apiResponse.Result = _mapper.Map<List<TaskDto>>(tasks);
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }catch(Exception ex)
            {
                _apiResponse.IsSuccess = false;
				_apiResponse.Errors = new List<string> { ex.Message.ToString() };
			}
            return _apiResponse;
        }
		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetById(int id)
        {
            try
            {
                if(id == 0)
                {
                    return BadRequest();
                }
                Task task = await _task.GetAsync(t=>t.Id == id,"Category");
                if(task == null)
                {
                    return NotFound();
                }
                _apiResponse.Result = _mapper.Map<TaskDto>(task);
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);

            }catch(Exception ex)
            {
				_apiResponse.IsSuccess = false;
				_apiResponse.Errors = new List<string> { ex.Message.ToString() };
			}
            return _apiResponse;
        }
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Create([FromBody]TaskCreateDto taskDto)
        {
            try
            {
				Task tasks = await _task.GetAsync(t=>t.Title.ToLower()==taskDto.Title.ToLower());
                if(tasks != null)
                {
					ModelState.AddModelError("", "task is exist");
					return BadRequest(ModelState);
				}
                if(taskDto == null)
                {
                    return BadRequest(taskDto);
                }
                
				Task task = _mapper.Map<Task>(taskDto);
				task.CreatedAt = DateTime.UtcNow;
				task.IsCompleted = false;
				task.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				await _task.CreateAsync(task);
                await _task.SaveAsync();
                _apiResponse.Result = _mapper.Map<TaskDto>(task);
                _apiResponse.StatusCode = HttpStatusCode.Created;
				return Ok(_apiResponse);

			}
			catch(Exception ex)
            {
                _apiResponse.IsSuccess = false;
				_apiResponse.Errors = new List<string> { ex.Message.ToString() };
			}
            return _apiResponse;
        }
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if(id == 0)
                {
                    return BadRequest();
                }
                Task task = await _task.GetAsync(t=>t.Id == id);
                if (task == null)
                {
                    return NotFound();
                }
                await _task.RemoveAsync(task);
                await _task.SaveAsync();
                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }catch(Exception ex)
            {
                _apiResponse.IsSuccess = false;
				_apiResponse.Errors = new List<string> { ex.Message.ToString() };
			}
            return _apiResponse;
        }
		[HttpPut("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> Update(int id, [FromBody] TaskUpdateDto taskDto)
        {
            try
            {
                if(taskDto == null || id == 0||id != taskDto.Id)
                {
                    return BadRequest();
                }
                var exist = await _task.GetAsync(t=>t.Title == taskDto.Title&&t.Id!=taskDto.Id);
                if (exist != null)
                {
					ModelState.AddModelError("Title", "Task is already exist");
					_apiResponse.IsSuccess = false;
					_apiResponse.Errors = new List<string> { "Task is already exist" };
					return _apiResponse;
				}
                var createdat = await _task.GetAsync(t => t.Title == taskDto.Title,tracked:false);
				var task = _mapper.Map<Task>(taskDto);
                task.CreatedAt = createdat.CreatedAt;
                task.UserId = createdat.UserId;
                await _task.UpdateAsync(task);
                await _task.SaveAsync();
				_apiResponse.StatusCode = HttpStatusCode.NoContent;
				_apiResponse.IsSuccess = true;
				return Ok(_apiResponse);
			}
			catch(Exception ex)
            {
				_apiResponse.IsSuccess = false;
				_apiResponse.Errors = new List<string> { ex.Message.ToString() };
			}
			return _apiResponse;
		}
		[HttpPatch("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> UpdatePartial(int id, JsonPatchDocument<Task> patch)
        {
			if (patch == null || id == 0)
			{
				return BadRequest();
			}
            Task task = await _task.GetAsync(t=>t.Id==id);
            if (task == null)
            {
                return NotFound();
            }
            patch.ApplyTo(task,ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _task.UpdateAsync(task);
            await _task.SaveAsync();
            return NoContent();
		}
	}
}
