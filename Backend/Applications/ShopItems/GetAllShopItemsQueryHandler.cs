using MediatR;
using UGHApi.Entities;
using UGHApi.Interfaces;
using UGHApi.Shared;

namespace UGHApi.Applications.ShopItems
{
    public class GetAllShopItemsQueryHandler
        : IRequestHandler<GetAllShopItemsQuery, PaginatedList<ShopItem>>
    {
        private readonly IShopItemRepository _repository;

        public GetAllShopItemsQueryHandler(IShopItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<ShopItem>> Handle(
            GetAllShopItemsQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetAllAsync(request.PageNumber, request.PageSize);
        }
    }
}
