namespace UGHApi.Models;

public class StripeWebhookPolicyOptions
{
    public int PermitLimit { get; set; }
    public int QueueLimit { get; set; }
    public int WindowMinutes { get; set; }
}
