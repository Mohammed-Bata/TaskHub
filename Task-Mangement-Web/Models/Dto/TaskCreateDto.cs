using System.ComponentModel.DataAnnotations;

namespace Task_Mangement_Web.Models.Dto
{
    public class TaskCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
