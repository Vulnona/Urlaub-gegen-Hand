using MediatR;
using UGHApi.Entities;

namespace UGHApi.Applications.ShopItems;

public class UpdateShopItemCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Money Price { get; set; }
    public string Description { get; set; }
}
