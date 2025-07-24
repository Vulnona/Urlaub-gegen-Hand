using MediatR;
using UGH.Domain.Core;

namespace UGH.Application.Admin;

public class UpdateVerifyStateCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public UGH_Enums.VerificationState VerificationState { get; set; }

    public UpdateVerifyStateCommand(Guid userId, UGH_Enums.VerificationState verificationState)
    {
        UserId = userId;
        VerificationState = verificationState;
    }
}
