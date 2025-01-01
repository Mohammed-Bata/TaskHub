using Task_Mangement_Web.Models.Dto;

namespace Task_mangement_Web.Models.Dto
{
	public class LoginResponseDto
	{
		public UserDto ApplicationUser { get; set; }
		public string Token {  get; set; } 
	}
}
