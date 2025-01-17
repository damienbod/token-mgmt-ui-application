using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Ui.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly PhotoService _photoService;

    public IndexModel(PhotoService photoService)
    {
        _photoService = photoService;
    }

    [BindProperty]
    public byte[] Photo { get; set; } = [];

    public async Task OnGetAsync()
    {
        var photo = await _photoService.GetPhotoAsync();

        // OR
        //var accessToken = await HttpContext.GetUserAccessTokenAsync(
        //    new UserTokenRequestParameters
        //    {
        //        Scope = "myscope"
        //    });

        //var photo = await _photoService.GetPhotoAsync(accessToken.AccessToken!);

        if (!string.IsNullOrEmpty(photo))
        {
            Photo = Base64UrlEncoder.DecodeBytes(photo);
        }
    }
}
