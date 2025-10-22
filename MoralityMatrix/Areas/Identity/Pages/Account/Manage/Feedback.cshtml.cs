using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoralityMatrix.Data;
using MoralityMatrix.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MoralityMatrix.Areas.Identity.Pages.Account.Manage
{
    public class FeedbackModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<FeedbackModel> _logger;

        public FeedbackModel (
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<FeedbackModel> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [Display(Name = "Utilizator")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public string Email { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Câmpul {0} este obligatoriu.")]
            [DataType(DataType.Text)]
            [Display(Name = "Titlu")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Câmpul {0} este obligatoriu.")]
            [DataType(DataType.Text)]
            [Display(Name = "Feedback")]
            public string Text { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Username = userName;
            Email = email;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nu s-a putut încărca utilizatorul cu ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nu s-a putut încărca utilizatorul cu ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            await _emailSender.SendEmailAsync(
                    "morality.matrix.2024@gmail.com",
                    $"Feedback de la utilizatorul {user.UserName} cu email-ul {user.Email}",
                    $"<h2>{Input.Title}</h2><p>{Input.Text}</p>");

            _logger.LogInformation("User {UserName} sent a feedback message!", user.UserName);
            StatusMessage = "Mulțumim pentru feedback-ul tău!";
            return RedirectToPage();
        }
    }
}
