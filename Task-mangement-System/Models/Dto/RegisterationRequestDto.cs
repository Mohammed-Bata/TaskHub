using System.ComponentModel.DataAnnotations;

namespace Task_mangement_System.Models.Dto
{
	public class RegisterationRequestDto
	{
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string UserName { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		public string? Country {  get; set; }
		[Required]
		public string Role { get; set; }
	}
}
