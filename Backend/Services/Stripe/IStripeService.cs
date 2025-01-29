namespace UGHApi.Services.Stripe;

#pragma warning disable CS8632

public interface IStripeService
{
    Task<string?> GetClientSecretAsync(string transactionId);
}
