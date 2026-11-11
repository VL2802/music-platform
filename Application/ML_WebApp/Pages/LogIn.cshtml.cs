using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_WebApp.MDA;
using ML_LogicLayer.Services;
using Microsoft.Extensions.Logging;
using ML_LogicLayer;
using SharedLibraryML.CollectionSection;
using SharedLibraryML.MediaSection;
using SharedLibraryML;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using ML_LogicLayer.Security;

namespace ML_WebApp.Pages
{
    public class LogInModel : PageModel
    {
        [BindProperty]
        public MDAUserLogin UserLogin { get; set; }
        private readonly UserService _userService;
        [ViewData]
        public string ActionLogger {  get; set; }

        private readonly LoginAttemptService _loginAttemptService;
        private const string FailedLoginKey = "FailedLoginAttempts";



        private readonly ILogger<LogInModel> _logger;


        public LogInModel(UserService userService, ILogger<LogInModel> logger, LoginAttemptService loginAttemptService)
        {
            _userService = userService;
            _logger = logger;
            _loginAttemptService = loginAttemptService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            int failedAttempts =
                HttpContext.Session.GetInt32(FailedLoginKey) ?? 0;

            if (!_loginAttemptService.CanAttemptLogin(failedAttempts))
            {
                ActionLogger = "Too many failed login attempts. Please try again later.";

                _logger.LogWarning(
                    "Login blocked due to too many failed attempts for email {Email}",
                    UserLogin.Email
                );

                return Page();
            }

            try
            {
                User authenticateUser =
                    _userService.AuthenticateUser(UserLogin.Email, UserLogin.Password);

                HttpContext.Session.Remove(FailedLoginKey);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, authenticateUser.GetEmail()),
            new Claim(ClaimTypes.Name, authenticateUser.GetUsername()),
            new Claim(ClaimTypes.Role, authenticateUser.GetRole().ToString())
        };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(
                        new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
                    ),
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                        IsPersistent = true
                    });

                _logger.LogInformation(
                    "User login successful for email {Email}",
                    authenticateUser.GetEmail()
                );

                return authenticateUser.GetRole() == Role.ARTIST
                    ? RedirectToPage("/ArtistPage")
                    : RedirectToPage("/ListenerPage");
            }
            catch (ArgumentException)
            {
                failedAttempts++;
                HttpContext.Session.SetInt32(FailedLoginKey, failedAttempts);

                _logger.LogWarning(
                    "Failed login attempt {Attempt} for email {Email}",
                    failedAttempts,
                    UserLogin.Email
                );

                ActionLogger = "Invalid email or password.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error during login for email {Email}",
                    UserLogin.Email
                );

                ActionLogger = ExceptionHandler.Handle(ex);
                return Page();
            }
        }

    }
}
