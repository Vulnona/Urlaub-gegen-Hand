using MediatR;
using UGHApi.ViewModels;

namespace UGH.Application.Admin;

public class GetProfileQuery : IRequest<UserDTO>
{
    public Guid UserId { get; set; }

    public GetProfileQuery(Guid userId)
    {
        UserId = userId;
    }
}
