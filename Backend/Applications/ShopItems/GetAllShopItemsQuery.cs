using MediatR;
using UGHApi.Entities;
using UGHApi.Shared;

namespace UGHApi.Applications.ShopItems;

public class GetAllShopItemsQuery : IRequest<PaginatedList<ShopItem>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetAllShopItemsQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
