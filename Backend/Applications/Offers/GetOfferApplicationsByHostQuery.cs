using MediatR;
using UGH.Domain.ViewModels;
using UGH.Domain.Core;
using UGHApi.Shared;

namespace UGH.Application.Offers;

public class GetOfferApplicationsByHostQuery : IRequest<Result<PaginatedList<OfferApplicationDto>>>
{
    public Guid HostId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetOfferApplicationsByHostQuery(Guid hostId, int pageNumber, int pageSize)
    {
        HostId = hostId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
