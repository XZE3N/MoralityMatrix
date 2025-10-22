using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MoralityMatrix.Pages
{
    public class QuizEndModel : PageModel
    {
        public int FinalScore { get; set; }

        public void OnGet(int score)
        {
            FinalScore = score;
            HttpContext.Session.SetString("_LastVisitedPage", "QuizEnd");
        }
    }
}
