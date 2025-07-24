using Microsoft.Extensions.Logging;

namespace UGH.infrastructure.Services.Facebook;

public class FacebookService : IFacebookService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FacebookService> _logger;
    private readonly string _accessToken;
    private readonly string _groupId;

    public FacebookService(
        HttpClient httpClient,
        ILogger<FacebookService> logger,
        string accessToken,
        string groupId
    )
    {
        _httpClient = httpClient;
        _logger = logger;
        _accessToken = accessToken;
        _groupId = groupId;
    }

    public async Task<bool> PostToGroupAsync(string message)
    {
        try
        {
            var endpoint = $"https://graph.facebook.com/v15.0/{_groupId}/feed";
            var postData = new Dictionary<string, string>
            {
                { "message", message },
                { "access_token", _accessToken }
            };

            var content = new FormUrlEncodedContent(postData);
            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully posted to Facebook group.");
                return true;
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Failed to post to Facebook group: {errorResponse}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Exception occurred while posting to Facebook: {ex.Message} | StackTrace: {ex.StackTrace}"
            );
            return false;
        }
    }
}
