using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CourseUser
    {
        [Key]
        public int Id { get; set; }

        [ValidateNever]
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = new();
        public int CourseId { get; set; }

        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; } = new();
        public string ApplicationUserId { get; set; } = string.Empty;

        [Required]
        public int UserCredits { get; set; }
    }
}
