using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ui.Pages;

[AllowAnonymous]
public class SignedOutModel : PageModel
{
    public void OnGet()
    {
    }
}