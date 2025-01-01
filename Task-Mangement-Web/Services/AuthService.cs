using Humanizer;
using System.Security.Cryptography.X509Certificates;
using System_Utility;
using Task_mangement_Web.Models.Dto;
using Task_Mangement_Web.Models;
using Task_Mangement_Web.Services.IServices;

namespace Task_Mangement_Web.Services
{
	public class AuthService:BaseService,IAuthService
	{
		//private readonly IHttpClientFactory _httpClient;
		private string taskurl;
		public AuthService(IHttpClientFactory httpClient,IConfiguration configuration):base(httpClient)
        {
            //_httpClient = httpClient;
			taskurl = configuration.GetValue<string>("ServiceUrls:TaskApi");
		}

        public Task<T> LoginAsync<T>(LoginRequestDto loginRequestDto)
		{
			return SendAsync<T>(new APIRequest()
			{
				apiType = SD.APIType.POST,
				Data = loginRequestDto,
				Url = taskurl + "/api/ApplicationUser/Login",
			});
		}

		public Task<T> RegisterAsync<T>(RegisterationRequestDto registerationRequestDto)
		{
			return SendAsync<T>(new APIRequest()
			{
				apiType = SD.APIType.POST,
				Data = registerationRequestDto,
				Url = taskurl + "/api/ApplicationUser/Register",
			});
		}
	}
}
