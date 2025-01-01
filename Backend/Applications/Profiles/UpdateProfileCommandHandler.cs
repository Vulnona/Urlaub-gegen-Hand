using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;

namespace UGH.Application.Profile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly ILogger<UpdateProfileCommandHandler> _logger;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        TokenService tokenService,
        ILogger<UpdateProfileCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var userId = request.UserId;
            var profile = request.Profile;

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user is null)
            {
                return Result.Failure<UserProfile>(
                    Errors.General.InvalidOperation("User not Authorized")
                );
            }

            if (profile != null)
            {
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                user.DateOfBirth = profile.DateOfBirth;
                user.Gender = profile.Gender;
                user.Street = profile.Street;
                user.HouseNumber = profile.HouseNumber;
                user.PostCode = profile.PostCode;
                user.City = profile.City;
                user.State = profile.State;
                user.Country = profile.Country;
                user.Skills = profile.Skills;
                user.Hobbies = profile.Hobbies;

                await _userRepository.UpdateUserAsync(user);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<UserProfile>(
                Errors.General.InvalidOperation("Something went wrong while updating profile")
            );
        }
    }
}
