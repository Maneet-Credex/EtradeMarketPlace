using System.ComponentModel.DataAnnotations;

namespace tradeMarketPlace_Frontend.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(buyer|seller)$", ErrorMessage = "Type must be either 'buyer' or 'seller'")]
        public string? Type { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\W)(?=.*\d)(?!.*\s).{8,}$",
        ErrorMessage = "The password must be at least 8 characters long and contain at least one uppercase letter, one digit and one special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string? ContactNumber { get; set; }
        public string? OrganisationName { get; set; }

        public string Status { get; set; } = "inactive";
    }
}
