using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using ML_LogicLayer;
using Microsoft.Extensions.Logging;
using ML_LogicLayer.Services;
using ML_WebApp.MDA;
using SharedLibraryML; 
namespace ML_WebApp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;

        private readonly UserService _userService;
        [BindProperty]
        public MDAUserRegistration UserRegister { get; set; }
        [ViewData]
        public string ActionLogger { get; set; }

        public RegisterModel(UserService userService, ILogger<RegisterModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                if (UserRegister.Password != UserRegister.RepeatPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    return Page();
                }
                User newUser = new User(UserRegister.Username, UserRegister.Password, UserRegister.Email, UserRegister.PhoneNumber, UserRegister.Role, ""); // role Artist is already here 
                _userService.CreateUser(newUser);
                _logger.LogInformation(
                   "User registration successful for email {Email}",
                   newUser.GetEmail()
               );
                return RedirectToPage("/LogIn");
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                _logger.LogWarning(
                    ex,
                    "User registration failed for email {Email}",
                    UserRegister.Email
                );
                return Page();
            }

        }
    }
}
