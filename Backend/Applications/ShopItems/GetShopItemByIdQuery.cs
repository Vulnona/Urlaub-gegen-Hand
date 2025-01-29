using MediatR;
using UGHApi.Entities;

namespace UGHApi.Applications.ShopItems;

public class GetShopItemByIdQuery : IRequest<ShopItem>
{
    public int Id { get; set; }
}
