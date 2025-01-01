using System.ComponentModel.DataAnnotations;


namespace Task_mangement_System.Models.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public CategoryDto Category { get; set; }
		public DateTime CreatedAt { get; set; }
        public bool IsCompleted {  get; set; }
	}
}
