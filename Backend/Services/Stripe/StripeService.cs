using System.Net.Http.Headers;
using System.Text.Json;

namespace UGHApi.Services.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public StripeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetClientSecretAsync(string transactionId)
        {
            var stripeEndpoint = $"https://api.stripe.com/v1/payment_intents/{transactionId}";
            var stripeSecretKey = _configuration["Stripe:ApiKey"];

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, stripeEndpoint);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                stripeSecretKey
            );

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Stripe API call failed with status code: {response.StatusCode}"
                );
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseContent);
            document.RootElement.TryGetProperty("status", out var intentStatus);

            if (intentStatus.ToString() == "succeeded")
            {
                throw new Exception($"Payment already completed!");
            }

            if (intentStatus.ToString() == "requires_payment_method")
            {
                if (
                    document.RootElement.TryGetProperty(
                        "client_secret",
                        out var clientSecretElement
                    )
                )
                {
                    return clientSecretElement.GetString();
                }
            }

            return null;
        }
    }
}
