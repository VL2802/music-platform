using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_LogicLayer.Services;

namespace ML_WebApp.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly PasswordResetService _passwordResetService;

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public ForgotPasswordModel(PasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        public IActionResult OnPost()
        {
            _passwordResetService.RequestPasswordReset(Email);

            // Same message regardless of email existence
            Message = "If an account exists, a password reset link has been sent.";

            return Page();
        }
    }
}
