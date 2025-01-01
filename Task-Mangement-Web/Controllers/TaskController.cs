using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System_Utility;
using Task_mangement_Web.Models;
using Task_mangement_Web.Models.Dto;
using Task_Mangement_Web.Models.Dto;
using Task_Mangement_Web.Services;
using Task_Mangement_Web.Services.IServices;

namespace Task_Mangement_Web.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        public TaskController(ITaskService taskService,ICategoryService categoryService, IMapper mapper)
        {
            _taskService = taskService;
            _categoryService = categoryService;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index()
        {
            List<TaskDto> list = new();
            var response = await _taskService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<TaskDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> CreateTask()
        {
            List<CategoryDto> list = new();
            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));
            }
            ViewBag.SelectList = new SelectList(list, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(TaskCreateDto taskDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _taskService.CreateAsync<APIResponse>(taskDto, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Task created successfully";
                    return RedirectToAction("Index");
                }
            }
            TempData["error"] = "Error";
            return View(taskDto);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var response = await _taskService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<TaskDto>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask(TaskDto taskDto)
        {
            var response = await _taskService.DeleteAsync<APIResponse>(taskDto.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Task deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error";
            return View(taskDto);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTask(int id)
        {
            List<CategoryDto> list = new();
            var response2 = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response2 != null && response2.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response2.Result));
            }
            var response = await _taskService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<TaskDto>(Convert.ToString(response.Result));
                TaskUpdateDto model2 = _mapper.Map<TaskUpdateDto>(model);
                model2.CategoryId = model.Category?.Id;
                ViewBag.SelectList = new SelectList(list, "Id","Name",model2.CategoryId);
                return View(model2);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Updatetask(TaskUpdateDto taskDto)
        {
            if (ModelState.IsValid)
            {
                APIResponse response = await _taskService.UpdateAsync<APIResponse>(taskDto, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {

                    TempData["success"] = "Task updated successfully";
                    return RedirectToAction("Index");
                }
            }
            TempData["error"] = "Error";
            return View(taskDto);
        }
    }
}
