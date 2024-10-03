using MediatR;
using UGH.Domain.ViewModels;
using UGH.Domain.Core;

namespace UGH.Application.Offers;

public class GetOfferApplicationsByHostQuery : IRequest<Result<List<OfferApplicationDto>>>
{
    public Guid HostId { get; }

    public GetOfferApplicationsByHostQuery(Guid hostId)
    {
        HostId = hostId;
    }
}
