using UGH.Domain.Entities;
using MediatR;
using UGH.Domain.ViewModels;

namespace UGH.Application.Profile;

public class GetUserProfileQuery : IRequest<UserProfileDataDTO>
{
    public Guid UserId { get; set; }

    public GetUserProfileQuery(Guid userId)
    {
        UserId = userId;
    }
}
