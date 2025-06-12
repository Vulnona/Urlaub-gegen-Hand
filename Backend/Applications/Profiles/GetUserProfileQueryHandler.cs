using UGH.Infrastructure.Services;
using UGH.Domain.ViewModels;
using UGH.Domain.Interfaces;
using MediatR;

namespace UGH.Application.Profile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDataDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserProfileQueryHandler> _logger;

    public GetUserProfileQueryHandler(
        ILogger<GetUserProfileQueryHandler> logger,
        IUserRepository userRepository
    )
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<UserProfileDataDTO> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var userId = request.UserId;

            var user = await _userRepository.GetUserWithRatingByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning($"User with ID {userId} not found.");
                return null;
            }

            var membershipEndDate = user.UserMemberships
                .Where(m => m.IsMembershipActive)
                .OrderBy(m => m.CreatedAt)
                .FirstOrDefault()?.Expiration;

            var userProfileDataDTO = new UserProfileDataDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = user.ProfilePicture,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Street = user.Street,
                HouseNumber = user.HouseNumber,
                PostCode = user.PostCode,
                City = user.City,
                State = user.State,
                Country = user.Country,
                FacebookLink = user.Facebook_link,
                Link_RS = user.Link_RS,
                Link_VS = user.Link_VS,
    //            UserRating = user.Offers.Any(o => o.AverageRating > 0)
    //? user.Offers.Where(o => o.AverageRating > 0).Average(o => o.AverageRating)
    //: 0.0,
                MembershipEndDate = membershipEndDate,
                Hobbies = user?.Hobbies?.Split(',').ToList(),
                Skills = user?.Skills?.Split(',').ToList(),
                VerificationState = user.VerificationState.ToString()
            };

            return userProfileDataDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"Error occurred while handling GetUserProfileQuery for UserId: {request.UserId}"
            );
            throw;
        }
    }
}

// using UGH.Infrastructure.Services;
// using UGH.Domain.ViewModels;
// using UGH.Domain.Interfaces;
// using MediatR;

// namespace UGH.Application.Profile;

// public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDataDTO>
// {
//     private readonly IUserRepository _userRepository;
//     private readonly ILogger<GetUserProfileQueryHandler> _logger;

//     public GetUserProfileQueryHandler(
//         ILogger<GetUserProfileQueryHandler> logger,
//         IUserRepository userRepository
//     )
//     {
//         _logger = logger;
//         _userRepository = userRepository;
//     }

//     public async Task<UserProfileDataDTO> Handle(
//         GetUserProfileQuery request,
//         CancellationToken cancellationToken
//     )
//     {
//         try
//         {
//             var userId = request.UserId;

//             var user = await _userRepository.GetUserWithRatingByIdAsync(userId);

//             if (user == null)
//             {
//                 _logger.LogWarning($"User with ID {userId} not found.");
//                 return null;
//             }

//             var userProfileDataDTO = new UserProfileDataDTO
//             {
//                 FirstName = user.FirstName,
//                 LastName = user.LastName,
//                 ProfilePicture = user.ProfilePicture,
//                 DateOfBirth = user.DateOfBirth,
//                 Gender = user.Gender,
//                 Street = user.Street,
//                 HouseNumber = user.HouseNumber,
//                 PostCode = user.PostCode,
//                 City = user.City,
//                 State = user.State,
//                 Country = user.Country,
//                 FacebookLink = user.Facebook_link,
//                 Link_RS = user.Link_RS,
//                 Link_VS = user.Link_VS,
//                 Hobbies = user?.Hobbies?.Split(',').ToList(),
//                 Skills = user?.Skills?.Split(',').ToList(),
//                 VerificationState = user.VerificationState.ToString()
//             };

//             return userProfileDataDTO;
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(
//                 ex,
//                 $"Error occurred while handling GetUserProfileQuery for UserId: {request.UserId}"
//             );
//             throw;
//         }
//     }
// }
