using System.ComponentModel.DataAnnotations;
using ML_LogicLayer;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;
using SharedLibraryML;

namespace ML_WebApp.MDA
{
    public class MDAUserBase
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Username length must be between {2} and {1}", MinimumLength = 4)]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public IFormFile? FileImage { get; set; }

    }

    public class MDAUserRegistration : MDAUserBase
    {
        public Role Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password length must be between {2} and {1}", MinimumLength = 14)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{14,50}$",
            ErrorMessage = @"Password must be between 14 and 50 characters long, contain at least one uppercase letter,
                            one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string RepeatPassword { get; set; }
    }

    public class MDAUserLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class MDAUserUpdatePassword
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password length must be between {2} and {1}", MinimumLength = 14)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{14,50}$",
            ErrorMessage = @"Password must be between 14 and 50 characters long, contain at least one uppercase letter,
                            one lowercase letter, one digit, and one special character.")]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string RepeatNewPassword { get; set; }
    }

}
