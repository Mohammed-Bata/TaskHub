using System.ComponentModel.DataAnnotations;
using Task_Mangement_Web.Models;

namespace Task_mangement_Web.Models.Dto
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
        [Required(ErrorMessage = "Password is required.")]
		[PasswordValidation]
        public string Password { get; set; }
		public string? Country {  get; set; }
		[Required]
		public string Role { get; set; }
	}
}
