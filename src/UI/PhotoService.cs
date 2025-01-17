using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Ui;

public class PhotoService
{
    private readonly IOptions<AuthConfigurations> _authConfigurations;
    private readonly IHttpClientFactory _clientFactory;

    public PhotoService(IOptions<AuthConfigurations> authConfigurations,
        IHttpClientFactory clientFactory)
    {
        _authConfigurations = authConfigurations;
        _clientFactory = clientFactory;
    }

    /// <summary>
    /// HttpContext is used to get the access token and it is passed as a parameter
    /// </summary>
    public async Task<string> GetPhotoAsync(string accessToken)
    {
        try
        {
            var client = _clientFactory.CreateClient(); 
            client.BaseAddress = new Uri(_authConfigurations.Value.ProtectedApiUrl);
            client.SetBearerToken(accessToken);

            var response = await client.GetAsync("api/Profiles/photo");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                if (data != null)
                    return data;

                return string.Empty;
            }

            throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Exception {e}");
        }
    }

    /// <summary>
    /// Accesses API using named client and access token 
    /// </summary>

    public async Task<string> GetPhotoAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient("profileClient");

            var response = await client.GetAsync("api/Profiles/photo");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                if (data != null)
                    return data;

                return string.Empty;
            }

            throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Exception {e}");
        }
    }
}