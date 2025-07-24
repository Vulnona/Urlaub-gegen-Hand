using MediatR;

namespace UGHApi.Applications.ShopItems;

public class DeleteShopItemCommand : IRequest
{
    public int Id { get; set; }
}
