using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Formats.Asn1.AsnWriter;

namespace MoralityMatrix.Pages
{
    public class QuizStartModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public (Quote, Quote) RandomQuotesPair { get; private set; }

        public QuizStartModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            RandomQuotesPair = QuoteManager.GetRandomQuotesPair();
            HttpContext.Session.SetString("_LastVisitedPage", "QuizStart");
        }

        public IActionResult OnPost(int selectedOption)
        {
            int Chapter = selectedOption;
            return RedirectToPage("Quiz", new { Chapter });
        }
    }

    public class Quote
    {
        public string Text { get; set; }
        public string Author { get; set; }
    }

    public static class QuoteManager
    {
        private static List<Quote> quotes = new List<Quote>
        {
            new Quote { 
                Text = "Educația este cheia pentru a debloca lumea, cheia pentru libertate.",
                Author = "Oprah Winfrey"
            },
            new Quote {
                Text = "Cel mai mare secret în viață este că suntem ceea ce gândim că suntem.",
                Author = "Ralph Waldo Emerson"
            },
            new Quote { 
                Text = "Educația este cea mai puternică armă pe care o poți folosi pentru a schimba lumea.", 
                Author = "Nelson Mandela"
            },
            new Quote {
                Text = "Știința este un instrument minunat, dar nu înlocuiește învățarea.",
                Author = "William Pollard"
            },
            new Quote {
                Text = "Mintea care se deschide unei noi idei nu se va mai întoarce niciodată la dimensiunea sa originală.",
                Author = "Albert Einstein"
            },
            new Quote {
                Text = "Nimeni nu poate să îți fure educația.",
                Author = "B.B. King"
            },
            new Quote {
                Text = "Ceea ce știm este o picătură, ceea ce nu știm este un ocean.",
                Author = "Isaac Newton"
            },
            new Quote {
                Text = "Inspirat de cunoaștere, umblu pe drumul îngust al cercetării.",
                Author = "Confucius"
            },
            new Quote {
                Text = "Cel mai mare secret al vieții este să fii pregătit să înveți.",
                Author = "John Dewey"
            },
            new Quote {
                Text = "Cea mai înțeleaptă cunoaștere este să știi cât de mult nu știi.",
                Author = "Socrate"
            },
            new Quote {
                Text = "Cunoașterea începe cu uimirea.",
                Author = "Socrate"
            },
            new Quote {
                Text = "Nu știința este cunoaștere, ci acțiunea.",
                Author = "Heraclit din Efes"
            },
            new Quote {
                Text = "Adevărata cunoaștere vine doar prin intermediul experienței.",
                Author = "Platon"
            },
            new Quote {
                Text = "Cunoașterea de sine este începutul întelepciunii.",
                Author = "Aristotel"
            },
            new Quote {
                Text = "Cel mai de preț dar pe care-l poți primi este cunoașterea de sine.",
                Author = "Socrate"
            },
            new Quote {
                Text = "Cel care nu știe nimic nu poate înțelege nimic.",
                Author = "Aristotel"
            },
            // Add more quotes as needed
        };

        public static Quote GetRandomQuote()
        {
            Random random = new Random();
            int index = random.Next(quotes.Count);
            return quotes[index];
        }

        public static (Quote, Quote) GetRandomQuotesPair()
        {
            Quote firstQuote = GetRandomQuote();
            Quote secondQuote = GetRandomQuote();

            // Ensure that the second quote is different from the first one
            while (secondQuote == firstQuote)
            {
                secondQuote = GetRandomQuote();
            }

            return (firstQuote, secondQuote);
        }
    }
}
