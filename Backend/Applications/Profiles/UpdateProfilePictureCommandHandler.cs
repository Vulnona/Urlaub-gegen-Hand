using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;
using Mapster;
using UGHApi.ViewModels;

namespace UGH.Application.Profiles;

public class UpdateProfilePictureCommandHandler
    : IRequestHandler<UpdateProfilePictureCommand, Result<UserDTO>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateProfilePictureCommandHandler> _logger;

    public UpdateProfilePictureCommandHandler(
        IUserRepository userRepository,
        ILogger<UpdateProfilePictureCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<UserDTO>> Handle(
        UpdateProfilePictureCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // Check if ProfilePic exists
            if (request.ProfilePicture == null || request.ProfilePicture.Length <= 0)
            {
                return Result.Failure<UserDTO>(
                    Errors.General.InvalidOperation("Profile picture cannot be empty.")
                );
            }

            // Validate file type by checking magic numbers for JPEG and PNG
            if (!IsValidImageType(request.ProfilePicture))
            {
                return Result.Failure<UserDTO>(
                    Errors.General.InvalidOperation(
                        "Invalid file format. Only JPEG and PNG images are allowed."
                    )
                );
            }

            // Get user profile
            var existingProfile = await _userRepository.GetUserByIdAsync(request.UserId);
            if (existingProfile == null)
            {
                return Result.Failure<UserDTO>(
                    Errors.General.InvalidOperation("User profile not found.")
                );
            }

            // Update profile picture
            existingProfile.SetProfilePicture(request.ProfilePicture);

            // Save changes
            await _userRepository.UpdateUserAsync(existingProfile);

            // Map the User entity to UserDto using Mapster
            var userDto = existingProfile.Adapt<UserDTO>();

            return Result.Success(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<UserDTO>(
                Errors.General.InvalidOperation(
                    "Something went wrong while updating the profile picture."
                )
            );
        }
    }

    // Helper method to validate image types by checking the magic numbers
    private bool IsValidImageType(byte[] profilePicture)
    {
        // Magic numbers for JPEG and PNG
        var jpegSignature = new byte[] { 0xFF, 0xD8, 0xFF };
        var pngSignature = new byte[] { 0x89, 0x50, 0x4E, 0x47 };

        return profilePicture.Take(3).SequenceEqual(jpegSignature)
            || profilePicture.Take(4).SequenceEqual(pngSignature);
    }
}
