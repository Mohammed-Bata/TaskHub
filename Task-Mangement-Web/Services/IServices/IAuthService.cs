using Task_mangement_Web.Models.Dto;

namespace Task_Mangement_Web.Services.IServices
{
	public interface IAuthService
	{
		Task<T> LoginAsync<T>(LoginRequestDto loginRequestDto);
		Task<T> RegisterAsync<T>(RegisterationRequestDto registerationRequestDto);
	}
}
