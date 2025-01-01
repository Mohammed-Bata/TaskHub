using System.ComponentModel.DataAnnotations;

namespace Task_mangement_System.Models.Dto
{
	public class LoginRequestDto
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
