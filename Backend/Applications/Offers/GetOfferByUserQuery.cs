using UGH.Domain.Core;
using MediatR;
using UGH.Domain.ViewModels;

namespace UGH.Application.Offers;

public class GetOfferByUserQuery : IRequest<Result<List<OfferDTO>>>
{
    public Guid UserId { get; }

    public GetOfferByUserQuery(Guid userId)
    {
        UserId = userId;
    }
}
