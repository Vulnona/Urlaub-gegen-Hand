using MediatR;
using UGH.Domain.Core;
using UGH.Domain.ViewModels;

namespace UGH.Application.Offers;

public class AddOfferCommand : IRequest<Result>
{
    public OfferViewModel OfferViewModel { get; }
    public Guid UserId { get; }

    public AddOfferCommand(OfferViewModel offerViewModel, Guid userId)
    {
        OfferViewModel = offerViewModel;
        UserId = userId;
    }
}
