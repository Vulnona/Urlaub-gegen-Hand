using MediatR;
using UGH.Domain.Core;

namespace UGH.Application.Authentication;

public class RefreshTokenCommand : IRequest<Result>
{
    public string RefreshToken { get; }

    public RefreshTokenCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
