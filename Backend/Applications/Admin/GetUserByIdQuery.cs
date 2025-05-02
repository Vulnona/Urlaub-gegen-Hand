using UGH.Domain.ApplicationResponses;
using MediatR;

namespace UGH.Application.Admin;

public class GetUserByIdQuery : IRequest<UserDataResponse>
{
    public Guid UserId { get; }

    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}
