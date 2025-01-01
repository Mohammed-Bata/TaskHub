using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_mangement_System.Logging;
using Task_mangement_System.Models;
using Task_mangement_System.Models.Dto;
using Task_mangement_System.Repository.IRepository;

namespace Task_mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ICategoryRepository _category;
        private readonly ILogging _logger;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository category, ILogging logger, IMapper mapper)
        {
            _category = category;
            _logger = logger;
            _mapper = mapper;
            this._response = new APIResponse();
        }
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllCategories()
        {
            try
            {
                _logger.Log("Getting all categories", "");
                IEnumerable<Category> categories = await _category.GetAll();
                _response.Result = _mapper.Map<List<CategoryDto>>(categories);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message.ToString() };
            }
            return _response;
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult<APIResponse>> GetById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("Get category error with Id" + id, "error");
                    return BadRequest();
                }
                Category category = await _category.GetAsync(u => u.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<CategoryDto>(category);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message.ToString() };
            }
            return _response;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult<APIResponse>> Create([FromBody] CategoryCreateDto categorydto)
        {
            try
            {
                var categories = await _category.GetAsync(c => c.Name.ToLower() == categorydto.Name.ToLower());
                if (categories != null)
                {
                    ModelState.AddModelError("", "category is exist");
                    return BadRequest(ModelState);
                }

                if (categorydto == null)
                {
                    return BadRequest(categorydto);
                }

                Category category = _mapper.Map<Category>(categorydto);
                
                await _category.CreateAsync(category);
                await _category.SaveAsync();
                _response.Result = _mapper.Map<CategoryDto>(category);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message.ToString() };
            }
            return _response;
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                Category category = await _category.GetAsync(c => c.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                await _category.RemoveAsync(category);
                await _category.SaveAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message.ToString() };
            }
            return _response;
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult<APIResponse>> Update(int id, [FromBody]CategoryDto categoryDto)
        {
            try
            {
                if (id == 0 || categoryDto == null||id!=categoryDto.Id)
                {
                    return BadRequest();
                }
                var exist = await _category.GetAsync(c=>c.Name==categoryDto.Name&&c.Id!=categoryDto.Id);
                if(exist!=null)
                {
                    ModelState.AddModelError("Name", "Category is already exist");
                    _response.IsSuccess = false;
					_response.Errors = new List<string> { ModelState["Name"].ToString() };
                    return _response;
				}
                var category = _mapper.Map<Category>(categoryDto);
                await _category.UpdateAsync(category);
                await _category.SaveAsync();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message.ToString() };
            }
            return _response;
        }
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> UpdatePartial(int id,JsonPatchDocument<Category> patch)
        {
            if(patch == null || id == 0)
            {
                return BadRequest();
            }
            var category = await _category.GetAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            patch.ApplyTo(category,ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.UpdateAsync(category);
            await _category.SaveAsync();

            return NoContent();
        }

    }
}
