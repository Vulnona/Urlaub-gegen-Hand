using MediatR;
using UGHApi.Entities;
using UGH.Domain.Interfaces;

namespace UGHApi.Applications.ShopItems;

public class UpdateShopItemCommandHandler : IRequestHandler<UpdateShopItemCommand>
{
    private readonly IShopItemRepository _repository;

    public UpdateShopItemCommandHandler(IShopItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        UpdateShopItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var shopItem = new ShopItem
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
        };

        await _repository.UpdateAsync(shopItem);

        return Unit.Value;
    }
}
