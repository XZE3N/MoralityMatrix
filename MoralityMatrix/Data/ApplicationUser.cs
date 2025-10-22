using Microsoft.AspNetCore.Identity;

namespace MoralityMatrix.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }

        public ApplicationUser()
        {
            FirstName = "";
            LastName = "";
        }
    }
}
