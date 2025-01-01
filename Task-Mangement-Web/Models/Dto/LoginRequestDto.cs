using System.ComponentModel.DataAnnotations;

namespace Task_mangement_Web.Models.Dto
{
	public class LoginRequestDto
	{
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
