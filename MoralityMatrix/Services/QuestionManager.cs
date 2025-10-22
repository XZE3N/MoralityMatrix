using MoralityMatrix.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MoralityMatrix.Services
{
    public class QuestionManager
    {
        private readonly ILogger<QuestionManager> _logger;
        private readonly ApplicationDbContext _context;
        private List<QuestionWithOptions> _questions { get; set; }

        public QuestionManager(ILogger<QuestionManager> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

            _questions = new List<QuestionWithOptions>();
            LoadQuestions();

            Questions = _questions;
        }

        public List<QuestionWithOptions> Questions { get; set; }

        public void LoadQuestions()
        {
            // Initialize a Question list
            var questionsWithOptions = new List<QuestionWithOptions>();

            // Retrieve the question entity from the database
            var questions = _context.Questions.ToList();
            foreach (var question in questions)
            {
                // Retrieve the associated option entities from the database
                var options = _context.Option.Where(o => o.QuestionId == question.Id).ToList();

                // Create and populate a list item entity
                var questionWithOptions = new QuestionWithOptions
                {
                    QuestionId = question.Id,
                    QuestionText = question.QuestionText,
                    Chapter = question.Chapter ?? -1,
                    CorrectOptionIndex = question.CorrectOptionIndex ?? -1,

                    // Populate the Options string list with the values of the associated options
                    Options = options.Select(o => o.Value ?? "").ToList()
                };
                // Add the item to the list
                questionsWithOptions.Add(questionWithOptions);
            }

            _questions = questionsWithOptions;
        }

        /// <summary>
        ///     The function sets the global Questions variable to the entire list of questions.
        /// </summary>
        public void GetQuestions()
        {
            Questions = _questions;
        }

        /// <summary>
        ///     The function sets the global Questions variable to a list of questions specifically
        ///     selected from a specified chapter.
        /// </summary>
        /// <param name="chapter">
        ///     An integer representing the ID of the chapter from which questions are selected.
        /// </param>
        public void GetChapterQuestions(int? chapter)
        {
            Questions = _questions.Where(o => o.Chapter == chapter).ToList();
        }

        /// <summary>
        ///     The function retrieves a list of questions tailored for starting a quiz, specifically
        ///     selected from a specified chapter. It ensures that only questions relevant to the chosen
        ///     chapter are included, optimizing the quiz's relevance and focus.
        /// </summary>
        /// <param name="chapter">
        ///     An integer representing the ID of the chapter from which questions are selected.
        /// </param>
        /// <returns>
        ///     A collection of Question entities retrieved from the database, filtered based on the
        ///     specified chapter. These questions are intended for use in initializing or starting a quiz.
        /// </returns>
        public List<QuestionWithOptions> GetChapterQuizQuestions(int? chapter)
        {
            return SelectQuizQuestions(_questions.Where(o => o.Chapter == chapter).ToList(), 5);
        }

        /// <summary>
        ///     The function retrieves a list of questions tailored for starting a quiz.
        /// </summary>
        /// <returns>
        ///     A collection of Question entities retrieved from the database. These questions are intended
        ///     for use in initializing or starting a quiz.
        /// </returns>
        public List<QuestionWithOptions> GetQuizQuestions()
        {
            return SelectQuizQuestions(_questions, 5);
        }

        private List<QuestionWithOptions> SelectQuizQuestions(List<QuestionWithOptions> questionList, int count)
        {
            Random random = new Random();

            // Select a number of random questions specified by the count parameter
            List<QuestionWithOptions> selectedQuestions = questionList.OrderBy(q => random.Next()).Take(count).ToList();
            
            return selectedQuestions;
        }

        /// <summary>
        ///     The function asynchronously creates a new question in the database with the provided details.
        ///     It facilitates the addition of a question along with its associated options, including the
        ///     correct option index and the chapter it belongs to.
        /// </summary>
        /// <param name="questionText">
        ///     A string representing the text of the question.
        /// </param>
        /// <param name="correctOptionIndex">
        ///     An integer indicating the index of the correct option among the provided options.
        /// </param>
        /// <param name="chapter">
        ///     An integer representing the ID of the chapter to which the question belongs.
        /// </param>
        /// <param name="optionValues">
        ///     An array of strings containing the values of the options associated with the question.
        /// </param>
        /// <returns>
        ///     An asynchronous task representing the completion of the function. It does not return a value
        ///     directly but rather completes when the question creation process is finished.
        /// </returns>
        public async Task CreateQuestionAsync(string questionText, int? correctOptionIndex, int? chapter, string[] optionValues)
        {
            // Create and populate the Question entity
            var question = new Question
            {
                QuestionText = questionText,
                CorrectOptionIndex = correctOptionIndex,
                Chapter = chapter
            };

            // Create and populate the Option entities
            foreach (var optionValue in optionValues)
            {
                var option = new Option
                {
                    Value = optionValue,
                    Question = question
                };
                question.Options ??= new List<Option>(); // Ensure Options collection is initialized
                question.Options.Add(option);
            }

            // Add the Question entity to the context
            _context.Questions.Add(question);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///     The function asynchronously deletes a question and its associated options from the database
        ///     based on the provided question ID. It facilitates the removal of a specific question along
        ///     with any related options, maintaining data integrity within the database.
        /// </summary>
        /// <param name="questionId">
        ///     An integer representing the ID of the question to be deleted.
        /// </param>
        /// <returns>
        ///     An asynchronous task representing the completion of the function. It does not return a value
        ///     directly but rather completes when the deletion process is finished.
        /// </returns>
        public async Task DeleteQuestionByIdAsync(int questionId)
        {
            // Retrieve the question entity from the database based on the QuestionId
            var question = await _context.Questions.Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == questionId);

            if (question != null)
            {
                // Remove associated option entities
                _context.Option.RemoveRange(question.Options);

                // Remove the question entity
                _context.Questions.Remove(question);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }
    }

    // Intermediate Question entity to facilitate Quiz operations
    public class QuestionWithOptions
    {
        public int QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public int Chapter { get; set; }
        public int CorrectOptionIndex { get; set; }
        public List<string>? Options { get; set; }
    }
}
