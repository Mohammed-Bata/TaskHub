using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task_mangement_System.Models;
using Task_mangement_System.Models.Dto;
using Task_mangement_System.Repository.IRepository;

namespace Task_mangement_System.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationUserController : ControllerBase
	{
		private readonly IApplicationUserRepository _userRepository;
		protected APIResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserController(IApplicationUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
           _userRepository = userRepository;
			_response = new();
			_userManager = userManager;
        }
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody]LoginRequestDto loginRequestDto)
		{
			var loginresponse = await _userRepository.Login(loginRequestDto);
			if (loginresponse.ApplicationUser == null || string.IsNullOrEmpty(loginresponse.Token))
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
                if (_response.Errors == null)
                {
                    _response.Errors = new List<string>();
                }
                _response.Errors.Add("User Name or Password is incorrect");
				return BadRequest(_response);
			}
			_response.IsSuccess = true;
			_response.StatusCode = HttpStatusCode.OK;
			_response.Result = loginresponse;
			return Ok(_response);
		}
		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
		{
			bool isunique = _userRepository.IsUniqueUser(model.UserName);
			if (!isunique)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.Errors.Add("Username already exists");
                return BadRequest(_response);
			}
            var passwordValidationResult = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, null, model.Password);
            if (!passwordValidationResult.Succeeded)
            {
                foreach (var error in passwordValidationResult.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
                return BadRequest(ModelState);
            }
            var user = await _userRepository.Register(model);
			if (user == null)
			{
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.IsSuccess = false;
				_response.Errors.Add("Error while registeration");
				return BadRequest(_response);
			}
			_response.StatusCode = HttpStatusCode.OK;
			_response.IsSuccess = true;
			return Ok(_response);
		}
	}
}
