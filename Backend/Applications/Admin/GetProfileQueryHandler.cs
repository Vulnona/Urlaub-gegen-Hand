using MediatR;
using UGH.Domain.Interfaces;
using UGHApi.ViewModels;

namespace UGH.Application.Admin;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserDTO>
{
    private readonly IUserRepository _userRepository;

    public GetProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDTO> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _userRepository.GetUserDetailsByIdAsync(request.UserId);

        if (profile == null)
        {
            return null;
        }

        return profile;
    }
}
