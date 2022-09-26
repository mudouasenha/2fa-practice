using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Doodle.Presentation.Pages.Shared
{
    public class UserModel
    {
        public bool IsSignedIn { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class _LoginPartial : PageModel
    {
        public void OnGet()
        {
        }
    }
}