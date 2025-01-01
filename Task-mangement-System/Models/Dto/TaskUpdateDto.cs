using System.ComponentModel.DataAnnotations;

namespace Task_mangement_System.Models.Dto
{
	public class TaskUpdateDto
	{
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		public string Description { get; set; }
		public int? CategoryId { get; set; }
		public bool IsCompleted { get; set; }
	}
}
