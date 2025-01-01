using System.ComponentModel.DataAnnotations;

namespace Task_mangement_System.Models.Dto
{
	public class TaskCreateDto
	{
		[Required]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		public int? CategoryId { get; set; }
	}
}
