using System.ComponentModel.DataAnnotations;

namespace AgriEnergy_Connect.Models
{
    public class Register
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must be at least 6 characters, include a number and an uppercase letter.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role.")]
        public string Role { get; set; } = string.Empty;

        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Phone]
        [Display(Name = "Contact Number")]
        public string? ContactNumber { get; set; }

        public string? Location { get; set; }
    }
}
