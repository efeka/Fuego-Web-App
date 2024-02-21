using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class UserSchedule
    {
        [Key]
        public int ReservationId { get; set; }

        [ValidateNever]
        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; }
        public int ScheduleId { get; set; }

        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
