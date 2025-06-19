using MediatR;
using UGHApi.Entities;

namespace UGHApi.Applications.ShopItems;

public class CreateShopItemCommand : IRequest<int>
{
    public string Name { get; set; }
    public Money Price { get; set; }
    public int Duration { get; set; }
    public string Description { get; set; }
}
