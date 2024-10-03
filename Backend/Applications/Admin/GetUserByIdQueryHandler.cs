using UGH.Domain.ApplicationResponses;
using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using MediatR;

namespace UGH.Application.Admin;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDataResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _userProfileRepository;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        IUserProfileRepository userProfileRepository
    )
    {
        _userRepository = userRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<UserDataResponse> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetUserByIdAsync(request.UserId);

        if (user == null)
        {
            //return Result.Failure(Errors.General.InvalidOperation("User not found."));
            throw new Exception("User not found.");
        }

        var userProfile = await _userProfileRepository.GetUserProfileByUserIdAsync(request.UserId);

        var userData = new UGH.Domain.ApplicationResponses.UserDataResponse
        {
            User_Id = user.User_Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth.ToDateTime(TimeOnly.MinValue),
            Gender = user.Gender,
            Street = user.Street,
            HouseNumber = user.HouseNumber,
            PostCode = user.PostCode,
            City = user.City,
            Country = user.Country,
            State = user.State,
            Email_Address = user.Email_Address,
            IsEmailVerified = user.IsEmailVerified,
            MembershipId = user.MembershipId,
            Facebook_Link = user.Facebook_link,
            Link_RS = user.Link_RS,
            Link_VS = user.Link_VS,
            VerificationState = (int)user.VerificationState,
            CurrentMembership = user.CurrentMembership,
            AverageRating = user.AverageRating
        };

        return userData;
    }
}
