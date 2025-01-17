using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Ui;

/// <summary>
/// Cache persists token per application
/// </summary>
public class ApplicationAccessTokenCache
{
    private readonly ILogger<ApplicationAccessTokenCache> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    private static readonly object _lock = new();
    private readonly IDistributedCache _cache;

    private const int cacheExpirationInDays = 1;

    private class AccessTokenItem
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresIn { get; set; }
    }

    public ApplicationAccessTokenCache(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory,
        IDistributedCache cache)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
        _logger = loggerFactory.CreateLogger<ApplicationAccessTokenCache>();
        _cache = cache;
    }

    public async Task<string> GetApiToken(string clientId, string scope, string secret)
    {
        var accessToken = GetFromCache(clientId);

        if ((accessToken != null) && (accessToken.ExpiresIn > DateTime.UtcNow))
        {
            return accessToken.AccessToken;
        }

        _logger.LogDebug("GetApiToken new from secure token server for {clientId}", clientId);

        var newAccessToken = await GetInternalApiToken(clientId, scope, secret);
        AddToCache(clientId, newAccessToken);

        return newAccessToken.AccessToken;
    }

    private async Task<AccessTokenItem> GetInternalApiToken(string clientId, string scope, string secret)
    {
        try
        {
            var disco = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(
                _httpClient,
                _configuration["OpenIDConnectSettings:Authority"]);

            if (disco.IsError)
            {
                _logger.LogError("disco error Status code: {discoIsError}, Error: {discoError}", disco.IsError, disco.Error);
                throw new ApplicationException($"Status code: {disco.IsError}, Error: {disco.Error}");
            }

            var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(_httpClient, 
                new ClientCredentialsTokenRequest
                {
                    Scope = scope,
                    ClientSecret = secret,
                    Address = disco.TokenEndpoint,
                    ClientId = clientId
                });

            if (tokenResponse.IsError)
            {
                _logger.LogError("tokenResponse.IsError Status code: {tokenResponseIsError}, Error: {tokenResponseError}", tokenResponse.IsError, tokenResponse.Error);
                throw new ApplicationException($"Status code: {tokenResponse.IsError}, Error: {tokenResponse.Error}");
            }

            return new AccessTokenItem
            {
                ExpiresIn = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
                AccessToken = tokenResponse.AccessToken!
            };

        }
        catch (Exception e)
        {
            _logger.LogError("Exception {e}", e);
            throw new ApplicationException($"Exception {e}");
        }
    }

    private void AddToCache(string key, AccessTokenItem accessTokenItem)
    {
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromDays(cacheExpirationInDays));

        lock (_lock)
        {
            _cache.SetString(key, JsonSerializer.Serialize(accessTokenItem), options);
        }
    }

    private AccessTokenItem? GetFromCache(string key)
    {
        var item = _cache.GetString(key);
        if (item != null)
        {
            return JsonSerializer.Deserialize<AccessTokenItem>(item);
        }

        return null;
    }
}