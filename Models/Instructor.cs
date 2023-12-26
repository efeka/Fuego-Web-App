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
        // [RegularExpression(@"^\(\d{3}\)\s?\d{3}\s?\d{2}\s?\d{2}$")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        // [RegularExpression(@"^TR\d{2}(\s?\d{4}){3}\s?\d{4}\s?\d{4}\s?\d{4}\s?\d{4}\s?\d{2}$")]
        public string IBAN { get; set; } = string.Empty;

        [Required]
        public DateTime LastPayment { get; set; }
    }
}
