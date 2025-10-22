using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoralityMatrix.Data;
using MoralityMatrix.Services;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace MoralityMatrix.Pages
{
    [Authorize]
    public class QuizModel : PageModel
    {
        private List<QuestionWithOptions> _questions;
        private readonly QuestionManager _questionManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        private const string SessionKeyScore = "_Score";
        private const string SessionKeyChapter = "_Chapter";
        private const string SessionKeyQuestions = "_Questions";
        private const string SessionKeyCurrentQuestionIndex = "_CurrentQuestionIndex";

        public QuizModel(ApplicationDbContext context, ILogger<IndexModel> logger, QuestionManager questionManager)
        {
            _context = context;
            _logger = logger;
            _questionManager = questionManager;
        }

        /// <summary>
        ///     If the session variable is not set then initialize it with zero.
        ///     Handle the nullable int to regular int conversion using the C# ?? coalescing
        ///     operator which sets the value in the session variable or 0 if it is null.
        /// </summary>
        public int Score { 
            get => string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyScore)) ? 0 : (HttpContext.Session.GetInt32(SessionKeyScore) ?? 0); 
            set => HttpContext.Session.SetInt32(SessionKeyScore, value); 
        }

        /// <summary>
        ///     If the session variable is not set then initialize it with zero.
        ///     Handle the nullable int to regular int conversion using the C# ?? coalescing
        ///     operator which sets the value in the session variable or 0 if it is null.
        /// </summary>
        public int Chapter
        {
            get => string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyChapter)) ? 0 : (HttpContext.Session.GetInt32(SessionKeyChapter) ?? 0);
            set => HttpContext.Session.SetInt32(SessionKeyChapter, value);
        }

        /// <summary>
        ///     If the session variable is not set then initialize it with zero.
        ///     Handle the nullable int to regular int conversion using the C# ?? coalescing
        ///     operator which sets the value in the session variable or 0 if it is null.
        /// </summary>
        public int CurrentQuestionIndex { 
            get => string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyCurrentQuestionIndex)) ? 0 : (HttpContext.Session.GetInt32(SessionKeyCurrentQuestionIndex) ?? 0); 
            set => HttpContext.Session.SetInt32(SessionKeyCurrentQuestionIndex, value); 
        }

        /// <summary>
        ///     If the session variable is not set then initialize it with zero.
        ///     Handle the nullable int to regular int conversion using the C# ?? coalescing
        ///     operator which sets the value in the session variable or 0 if it is null.
        /// </summary>
        public List<QuestionWithOptions> Questions
        {
            get => string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyQuestions)) ? new List<QuestionWithOptions>() : JsonSerializer.Deserialize<List<QuestionWithOptions>>(HttpContext.Session.GetString(SessionKeyQuestions));
            set => HttpContext.Session.SetString(SessionKeyQuestions, JsonSerializer.Serialize(value));
        }

        public int CurrentQuestionDisplayIndex => CurrentQuestionIndex + 1;

        public int TotalQuestions => _questions.Count;

        public QuestionWithOptions CurrentQuestion => _questions[CurrentQuestionIndex];

        public IActionResult OnGet(int chapter)
        {
            if(HttpContext.Session.GetString("_LastVisitedPage") != "Quiz")
            {
                if(chapter == 0)
                {
                    // Reset Quiz
                    Score = 0;
                    CurrentQuestionIndex = 0;
                    Chapter = 0;

                    // Redirect to home page
                    return RedirectToPage("/QuizStart");
                }
                else
                {
                    // Reset Quiz
                    Score = 0;
                    CurrentQuestionIndex = 0;
                    Chapter = 0;
                }
            }

            HttpContext.Session.SetString("_LastVisitedPage", "Quiz");

            if (chapter == 0 && Chapter == 0)
            {
                // Redirect to home page
                return RedirectToPage("/QuizStart");
            }
            
            // Initialize Session variables as needed
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyScore)))
            {
                Score = 0;
                CurrentQuestionIndex = 0;
            }
            if(string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyChapter)) || Chapter == 0)
            {
                Chapter = chapter;
                if(Chapter == -1)
                {
                    Questions = _questionManager.GetQuizQuestions();
                }
                else
                {
                    Questions = _questionManager.GetChapterQuizQuestions(Chapter);
                }
            }

            _questions = Questions;

            return Page();
        }

        public IActionResult OnPost(int selectedOption)
        {
            _questions = Questions;

            if (selectedOption == CurrentQuestion.CorrectOptionIndex)
            {
                Score++;
            }

            if (CurrentQuestionIndex + 1 < _questions.Count)
            { 
                CurrentQuestionIndex++;
            }
            else
            {
                // _logger.LogInformation("Quiz has been finished with score {Score}!", Score);

                // Reset Quiz
                int score = Score;
                Score = 0;
                CurrentQuestionIndex = 0;
                Chapter = 0;

                // Redirect to Quiz End
                return RedirectToPage("/QuizEnd", new { score });
            }

            return Page();
        }
    }
}
