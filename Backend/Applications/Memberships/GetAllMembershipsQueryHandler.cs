using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;

namespace UGHApi.Applications.Memberships;

public class GetAllMembershipsQueryHandler
    : IRequestHandler<GetAllMembershipsQuery, Result<List<Membership>>>
{
    private readonly IMembershipRepository _membershipRepository;

    public GetAllMembershipsQueryHandler(IMembershipRepository membershipRepository)
    {
        _membershipRepository = membershipRepository;
    }

    public async Task<Result<List<Membership>>> Handle(
        GetAllMembershipsQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var memberships = await _membershipRepository.GetAllMembershipsAsync();
            return Result.Success(memberships);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<Membership>>(
                Errors.General.InvalidOperation($"Error retrieving memberships: {ex.Message}")
            );
        }
    }
}
