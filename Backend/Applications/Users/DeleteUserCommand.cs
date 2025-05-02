using MediatR;
using UGH.Domain.Core;

namespace UGH.Application.Users;

public class DeleteUserCommand : IRequest<Result>
{
    public Guid UserId { get; set; }

    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }
}
