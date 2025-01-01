using Task_mangement_System.Models;
using Task_mangement_System.Models.Dto;

namespace Task_mangement_System.Repository.IRepository
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> UpdateAsync(ApplicationUser applicationUser);
		bool IsUniqueUser(string username);
		Task<ApplicationUserDto> Register(RegisterationRequestDto registerationRequestDto);
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
	}
}
