using MediatR;
using UGHApi.Entities;
using UGHApi.Extensions;
using UGHApi.Interfaces;

namespace UGHApi.Applications.ShopItems;

public class CreateShopItemCommandHandler : IRequestHandler<CreateShopItemCommand, int>
{
    private readonly IShopItemRepository _repository;

    public CreateShopItemCommandHandler(IShopItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        CreateShopItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var shopItem = new ShopItem
        {
            Name = request.Name,
            Price = request.Price,
            Duration = request.Duration.ToCouponDuration(),
            Description = request.Description,
        };

        return await _repository.CreateAsync(shopItem);
    }
}
