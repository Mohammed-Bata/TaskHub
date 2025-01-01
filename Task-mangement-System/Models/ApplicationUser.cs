using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Task_mangement_System.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        public string? Country { get; set; }
        [ValidateNever]
        public ICollection<Task> Tasks { get; set; }
    }
}
