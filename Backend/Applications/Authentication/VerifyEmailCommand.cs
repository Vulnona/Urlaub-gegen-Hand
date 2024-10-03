using MediatR;
using UGH.Domain.Core;

namespace UGH.Application.Authentication;

public class VerifyEmailCommand : IRequest<Result>
{
    public string Token { get; }

    public VerifyEmailCommand(string token)
    {
        Token = token;
    }
}
