using System.ComponentModel.DataAnnotations;

namespace Task_mangement_Web.Models.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
