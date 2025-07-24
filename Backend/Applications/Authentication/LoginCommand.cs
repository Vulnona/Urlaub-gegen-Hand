using MediatR;
using UGH.Contracts.Authentication;
using UGH.Domain.Core;

namespace UGH.Application.Authentication;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
