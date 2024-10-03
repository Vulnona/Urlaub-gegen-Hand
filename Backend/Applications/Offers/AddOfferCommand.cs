using MediatR;
using UGH.Domain.Core;
using UGH.Domain.ViewModels;

namespace UGH.Application.Offers;

public class AddOfferCommand : IRequest<Result>
{
    public OfferViewModel OfferViewModel { get; }

    public AddOfferCommand(OfferViewModel offerViewModel)
    {
        OfferViewModel = offerViewModel;
    }
}
