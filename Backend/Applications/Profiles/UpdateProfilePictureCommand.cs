using UGH.Domain.Core;
using MediatR;
using UGHApi.ViewModels;

namespace UGH.Application.Profiles;

public class UpdateProfilePictureCommand : IRequest<Result<UserDTO>>
{
    public Guid UserId { get; set; }
    public byte[] ProfilePicture { get; set; }

    public UpdateProfilePictureCommand(Guid userId, byte[] profilePicture)
    {
        UserId = userId;
        ProfilePicture = profilePicture;
    }
}
