using UGH.Domain.Entities;
using UGH.Domain.Core;
using MediatR;
using UGH.Domain.ViewModels;

namespace UGH.Application.Profile;

public class UpdateProfileCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public ProfileData Profile { get; set; }

    public UpdateProfileCommand(Guid userId, ProfileData profile)
    {
        UserId = userId;
        Profile = profile;
    }
}
