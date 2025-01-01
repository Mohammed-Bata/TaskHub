using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System_Utility;
using Task_mangement_Web.Models;
using Task_mangement_Web.Models.Dto;
using Task_Mangement_Web.Services;
using Task_Mangement_Web.Services.IServices;

namespace Task_Mangement_Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly IMapper _mapper;

        public AuthController(IAuthService authService,IMapper mapper)
        {
            _authService = authService;
			_mapper = mapper;
        }
		[HttpGet]
		public IActionResult Login()
		{
			LoginRequestDto obj = new LoginRequestDto();
			return View(obj);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginRequestDto obj)
		{
			APIResponse response = await _authService.LoginAsync<APIResponse>(obj);
			if(response != null && response.IsSuccess)
			{
				LoginResponseDto model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(model.Token);

				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

				identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
				identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
				var principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

				HttpContext.Session.SetString(SD.SessionToken,model.Token);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				if (response == null || !response.IsSuccess)
				{
					ModelState.AddModelError("CustomError", "User Name or Password is incorrect");
				}
				else
				{
					ModelState.AddModelError("CustomError", response.Errors.FirstOrDefault());
				}
				return View(obj);
			}
		}
		[HttpGet]
		public IActionResult Register()
		{
			List<string> roles = new List<string>() { "User" };
			ViewBag.SelectList = new SelectList(roles);
			RegisterationRequestDto obj = new RegisterationRequestDto();
			return View(obj);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterationRequestDto obj)
		{
            if (!ModelState.IsValid)
            {
                // Return the view with the current model, so errors can be displayed
                return View(obj);
            }
            APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);
			if(result != null || result.IsSuccess)
			{
				return RedirectToAction("Login");
			}
			return View();
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			HttpContext.Session.SetString(SD.SessionToken,"");
			return RedirectToAction("Index", "Home");
		}
		public IActionResult AccessDenied()
		{
			return View();
		}

	}
}
