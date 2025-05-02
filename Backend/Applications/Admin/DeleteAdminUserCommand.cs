using UGH.Domain.Core;
using MediatR;

namespace UGHApi.Applications.Admin;

public class DeleteAdminUserCommand : IRequest<Result>
{
    public Guid UserId { get; }

    public DeleteAdminUserCommand(Guid userId)
    {
        UserId = userId;
    }
}
