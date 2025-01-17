using IdentityModel.Client;

namespace Ui;

public class PhotoService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ApiTokenCacheClient _apiTokenCacheClient;

    public PhotoService(IConfiguration configuration,
        IHttpClientFactory clientFactory,
        ApiTokenCacheClient apiTokenCacheClient)
    {
        _configuration = configuration;
        _clientFactory = clientFactory;
        _apiTokenCacheClient = apiTokenCacheClient;
    }

    /// <summary>
    /// HttpContext is used to get the access token and it is passed as a parameter
    /// </summary>
    public async Task<string> GetPhotoAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["AuthConfigurations:ProtectedApiUrl"]!);

            var access_token = await _apiTokenCacheClient.GetApiToken(
                "CC",
                "myccscope",
                "cc_secret"
            );

            client.SetBearerToken(access_token);

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