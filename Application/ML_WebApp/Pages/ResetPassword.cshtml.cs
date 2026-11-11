using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_LogicLayer.Services;

namespace ML_WebApp.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly PasswordResetService _passwordResetService;

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public string Message { get; set; }

        public ResetPasswordModel(PasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        public IActionResult OnGet()
        {
            if (!_passwordResetService.ValidateToken(Token, out _))
                return RedirectToPage("/Error");

            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                _passwordResetService.ResetPassword(Token, NewPassword);
                return RedirectToPage("/LogIn");
            }
            catch
            {
                Message = "Invalid or expired reset link.";
                return Page();
            }
        }
    }
}
