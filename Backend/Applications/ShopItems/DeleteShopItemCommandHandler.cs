using MediatR;
using UGHApi.Interfaces;

namespace UGHApi.Applications.ShopItems;

public class DeleteShopItemCommandHandler : IRequestHandler<DeleteShopItemCommand>
{
    private readonly IShopItemRepository _repository;

    public DeleteShopItemCommandHandler(IShopItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        DeleteShopItemCommand request,
        CancellationToken cancellationToken
    )
    {
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
