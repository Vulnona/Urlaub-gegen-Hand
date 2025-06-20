using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGHApi.Entities;
using UGHApi.Interfaces;
using UGHApi.Models;

namespace UGHApi.Applications.Coupons;

public class CreatePaymentIntentCommandHandler
    : IRequestHandler<CreatePaymentIntentCommand, CreatePaymentIntentResponse>
{
    private readonly IShopItemRepository _shopItemRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly string _stripeApiKey;

    public CreatePaymentIntentCommandHandler(
        IShopItemRepository shopItemRepository,
        IUserRepository userRepository,
        ITransactionRepository transactionRepository,
        IConfiguration configuration
    )
    {
        _userRepository = userRepository;
        _shopItemRepository = shopItemRepository;
        _transactionRepository = transactionRepository;
        _stripeApiKey = configuration["Stripe:ApiKey"];
    }

    public async Task<CreatePaymentIntentResponse> Handle(
        CreatePaymentIntentCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
                throw new Exception("User not found.");

            var shopItem = await _shopItemRepository.GetByIdAsync(request.ShopItemId);

            if (shopItem == null)
                throw new Exception("Shop Item not found.");

            Stripe.StripeConfiguration.ApiKey = _stripeApiKey;

            var options = new Stripe.PaymentIntentCreateOptions
            {
                Amount = CalculateAmount(shopItem.Price.Amount),
                Currency = shopItem.Price.Currency,
                PaymentMethodTypes = new List<string> { "card", "paypal" },
            };

            var service = new Stripe.PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            var newTransaction = new Transaction
            {
                TransactionDate = DateTime.UtcNow,
                Amount = shopItem.Price,
                ShopItemId = shopItem.Id,
                UserId = user.User_Id,
                CouponId = null,
                TransactionId = paymentIntent.Id,
                Status = UGH_Enums.TransactionStatus.Pending,
            };

            var transaction = await _transactionRepository.CreateTransactionRecord(newTransaction);

            return new CreatePaymentIntentResponse { ClientSecret = paymentIntent.ClientSecret };
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create payment intent: {ex.Message}");
        }
    }

    private long CalculateAmount(decimal amount)
    {
        return (long)(amount * 100);
    }
}

