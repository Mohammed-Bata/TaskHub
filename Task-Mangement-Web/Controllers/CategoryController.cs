using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System_Utility;
using Task_mangement_Web.Models;
using Task_mangement_Web.Models.Dto;
using Task_Mangement_Web.Services.IServices;

namespace Task_Mangement_Web.Controllers
{
    [Authorize(Roles = "admin,User")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> IndexCategory()
        {
            List<CategoryDto> list = new();
            var response = await _categoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if(response != null&& response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
		public async Task<IActionResult> CreateCategory()
		{
            return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
		{
            if (ModelState.IsValid)
            {
				var response = await _categoryService.CreateAsync<APIResponse>(categoryDto,HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess)
				{
                    TempData["success"] = "Category created successfully";
                    return RedirectToAction("IndexCategory");
				}
			}
			TempData["error"] = "Error";
			return View(categoryDto);
		}
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.GetAsync<APIResponse>(id,HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCategory(CategoryDto categoryDto)
        {
            var response = await _categoryService.DeleteAsync<APIResponse>(categoryDto.Id,HttpContext.Session.GetString(SD.SessionToken));
            if(response != null && response.IsSuccess)
            {
				TempData["success"] = "Category deleted successfully";
				return RedirectToAction("IndexCategory");
            }
			TempData["error"] = "Error";
			return View(categoryDto);
        }

		[HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCategory(int id)
		{
			var response = await _categoryService.GetAsync<APIResponse>(id,HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				var model = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString(response.Result));
				return View(model);
			}
			return NotFound();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
		{
            if (ModelState.IsValid)
            {
				APIResponse response = await _categoryService.UpdateAsync<APIResponse>(categoryDto,HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
					TempData["success"] = "Category updated successfully";
					return RedirectToAction("IndexCategory");
                }
            }
            TempData["error"] = "Error";
			return View(categoryDto);
		}

	}
}
