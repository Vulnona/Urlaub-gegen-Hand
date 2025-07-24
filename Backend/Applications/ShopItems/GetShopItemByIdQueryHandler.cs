using MediatR;
using UGHApi.Entities;
using UGH.Domain.Interfaces;

namespace UGHApi.Applications.ShopItems;

public class GetShopItemByIdQueryHandler : IRequestHandler<GetShopItemByIdQuery, ShopItem>
{
    private readonly IShopItemRepository _repository;

    public GetShopItemByIdQueryHandler(IShopItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShopItem> Handle(
        GetShopItemByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}
