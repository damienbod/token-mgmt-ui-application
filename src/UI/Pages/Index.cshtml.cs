using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Ui.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationUsersService _photoService;

    public IndexModel(ApplicationUsersService photoService)
    {
        _photoService = photoService;
    }

    [BindProperty]
    public byte[] Photo { get; set; } = [];

    public async Task OnGetAsync()
    {
        var photo = await _photoService.GetPhotoAsync();

        if (!string.IsNullOrEmpty(photo))
        {
            Photo = Base64UrlEncoder.DecodeBytes(photo);
        }
    }
}
