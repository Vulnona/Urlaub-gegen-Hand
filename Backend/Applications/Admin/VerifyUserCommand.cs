using System.ComponentModel.DataAnnotations;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Admin;

public class VerifyUserCommand : IRequest<Result>
{
    [Required]
    public Guid UserId { get; set; }

    public VerifyUserCommand(Guid userId)
    {
        UserId = userId;
    }
}
