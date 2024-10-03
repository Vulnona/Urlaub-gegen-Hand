using MediatR;
using UGH.Domain.Core;


namespace UGH.Application.Authentication;

public class ResendVerificationEmailCommand : IRequest<Result>
{
    public string Email { get; }

    public ResendVerificationEmailCommand(string email)
    {
        Email = email;
    }
}
