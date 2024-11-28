using MediatR;

namespace UGH.Application.Authentication;

public class VerifyEmailCommand : IRequest<(string Html, string MimeType)>
{
    public string Token { get; }

    public VerifyEmailCommand(string token)
    {
        Token = token;
    }
}
