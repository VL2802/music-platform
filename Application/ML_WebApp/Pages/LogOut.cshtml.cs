using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ML_WebApp.Pages
{
    public class LogOutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "logout")
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/Index");
            }
            if (action == "cancel")
            {
                return RedirectToPage("/ArtistPage");
            }

            return Page();
        }
    }
}
