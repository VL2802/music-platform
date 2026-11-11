using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_LogicLayer.Services;
using ML_LogicLayer;

namespace ML_WebApp.Pages
{
    
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("ARTIST"))
                {
                    Response.Redirect("/ArtistPage");
                }
                if (User.IsInRole("LISTENER"))
                {
                    Response.Redirect("/ListenerPage");
                }



            }
        }
    }
}
