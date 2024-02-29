using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [ValidateNever]
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        public int CourseId { get; set; }

        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly Time { get; set; }
    }
}
