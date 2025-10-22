using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MoralityMatrix.Pages
{
    public class GuideModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.SetString("_LastVisitedPage", "Guide");
        }
    }
}
