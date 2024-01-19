using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Url]
        public string InstagramLink { get; set; } = string.Empty;

        [DisplayName("Last Payment Date")]
        public DateTime? LastPayment { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
