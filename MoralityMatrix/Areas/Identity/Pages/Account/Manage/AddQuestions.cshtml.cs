using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MoralityMatrix.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MoralityMatrix.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Admin")]
    public class AddQuestionsModel : PageModel
    {
        private readonly ILogger<AddQuestionsModel> _logger;
        private readonly QuestionManager _questionManager;

        public AddQuestionsModel(ILogger<AddQuestionsModel> logger, QuestionManager questionManager)
        {
            _logger = logger;
            _questionManager = questionManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Câmpul {0} este obligatoriu.")]
            [Display(Name = "Întrebare")]
            public string QuestionText { get; set; }

            [Required(ErrorMessage = "Câmpul {0} este obligatoriu.")]
            [Range(1, 12, ErrorMessage = "Câmpul {0} nu este valid.")]
            [RegularExpression("^[0-9]+$", ErrorMessage = "Câmpul {0} trebuie să conțină doar cifre.")]
            [Display(Name = "Capitol")]
            public int Chapter { get; set; }

            //[Range(1, 12, ErrorMessage = "Câmpul {0} nu este valid.")]
            [Required(ErrorMessage = "Câmpul Opțiune corectă este obligatoriu.")]
            public int CorrectOption { get; set; }

            // Optiuni 

            [Required(ErrorMessage = "Opțiunea unu este obligatorie.")]
            [Display(Name = "Opțiunea unu")]
            public string Option1 { get; set; }

            [Required(ErrorMessage = "Opțiunea doi este obligatorie.")]
            [Display(Name = "Opțiunea doi")]
            public string Option2 { get; set; }

            [Required(ErrorMessage = "Opțiunea trei este obligatorie.")]
            [Display(Name = "Opțiunea trei")]
            public string Option3 { get; set; }

            [Required(ErrorMessage = "Opțiunea patru este obligatorie.")]
            [Display(Name = "Opțiunea patru")]
            public string Option4 { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string[] options = { Input.Option1, Input.Option2, Input.Option3, Input.Option4 };

            await _questionManager.CreateQuestionAsync(Input.QuestionText, Input.CorrectOption, Input.Chapter, options);
            _logger.LogInformation("Question has been added successfully.");
            StatusMessage = "Întrebarea a fost creată cu succes!";
            return RedirectToPage();
        }
    }
}