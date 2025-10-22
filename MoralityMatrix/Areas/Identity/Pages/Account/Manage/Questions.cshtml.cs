using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoralityMatrix.Data;
using MoralityMatrix.Services;
using System.ComponentModel.DataAnnotations;

namespace MoralityMatrix.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Admin")]
    public class QuestionsModel : PageModel
    {
        private readonly ILogger<QuestionsModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly QuestionManager _questionManager;

        public List<QuestionWithOptions> Questions => _questionManager.Questions;

        public string Heading(int i)
        {
            return "heading" + i.ToString();
        }

        public string Collapse(int i)
        {
            return "collapse" + i.ToString();
        }

        public QuestionsModel(ApplicationDbContext context, QuestionManager questionManager, ILogger<QuestionsModel> logger)
        {
            _context = context;
            _questionManager = questionManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Display(Name = "Capitol")]
            public int Chapter { get; set; }

            [Display(Name = "Id Întrebare")]
            public int QuestionId { get; set; }
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(string formType)
        {
            if(formType == null)
            {
                return Page();
            }
            
            if(formType == "form1")
            {
                _logger.LogWarning(Input.Chapter.ToString());
                int Chapter = Input.Chapter;
                if (Chapter == 0)
                {
                    _questionManager.GetQuestions();
                }
                else
                {
                    _questionManager.GetChapterQuestions(Chapter);
                }
                return Page();
            }
            else if(formType == "form2")
            {
                if(Input.QuestionId == 0)
                {
                    return Page();
                }

                await _questionManager.DeleteQuestionByIdAsync(Input.QuestionId);
                _logger.LogInformation("Question {QuestionId} has been deleted successfully.", Input.QuestionId);
                StatusMessage = "Întrebarea numărul " + Input.QuestionId + " a fost ștearsă cu succes!";
                return RedirectToPage();
            }
            else
            {
                return Page();
            }
        }
    }
}
