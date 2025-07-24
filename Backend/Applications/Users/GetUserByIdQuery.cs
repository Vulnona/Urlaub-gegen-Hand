using UGH.Domain.Core;
using MediatR;
using UGHApi.ViewModels;

namespace UGH.Application.Users;

public class GetUserByIdQuery : IRequest<Result<UserDTO>>
{
    public Guid UserId { get; }

    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}
