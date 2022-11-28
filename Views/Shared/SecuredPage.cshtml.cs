using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogProject.Views.Shared
{
    [Authorize]
    public class SecuredPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
