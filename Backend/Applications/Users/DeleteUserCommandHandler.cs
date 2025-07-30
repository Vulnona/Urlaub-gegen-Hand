using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGHApi.DATA;
using Backend.Models;
using UGH.Domain.Entities;

namespace UGH.Application.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly S3Service _s3Service;
    private readonly Ugh_Context _context;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        ILogger<DeleteUserCommandHandler> logger,
        S3Service s3Service,
        Ugh_Context context
    )
    {
        _userRepository = userRepository;
        _logger = logger;
        _s3Service = s3Service;
        _context = context;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                return Result.Failure(Errors.General.InvalidOperation("User not found."));
            }

            // Create backup of user data before deletion
            await CreateUserBackup(user);

            var deleteTasks = new List<Task>();

            if (!string.IsNullOrEmpty(user.Link_RS))
                deleteTasks.Add(_s3Service.DeleteFileAsync(ExtractKeyFromUrl(user.Link_RS)));

            if (!string.IsNullOrEmpty(user.Link_VS))
                deleteTasks.Add(_s3Service.DeleteFileAsync(ExtractKeyFromUrl(user.Link_VS)));

            await Task.WhenAll(deleteTasks);

            await _userRepository.DeleteUserAsync(user.User_Id);

            _logger.LogInformation("User deleted successfully.");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation("An error occurred while deleting the user.")
            );
        }
    }

    private async Task CreateUserBackup(User user)
    {
        try
        {
            var backup = new DeletedUserBackup
            {
                UserId = user.User_Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                Email = user.Email_Address,
                Skills = user.Skills,
                Hobbies = user.Hobbies,
                ProfilePicture = user.ProfilePicture != null ? Convert.ToBase64String(user.ProfilePicture) : null,
                DeletedAt = DateTime.UtcNow
            };

            // Add address information if available
            if (user.Address != null)
            {
                backup.Address = $"{user.Address.Street} {user.Address.HouseNumber}, {user.Address.Postcode} {user.Address.City}";
                backup.Latitude = user.Address.Latitude;
                backup.Longitude = user.Address.Longitude;
            }

            _context.DeletedUserBackups.Add(backup);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User backup created for user {user.User_Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to create user backup: {ex.Message}");
            // Don't throw here, as we still want to delete the user even if backup fails
        }
    }

    private string ExtractKeyFromUrl(string url)
    {
        var uri = new Uri(url);
        return uri.LocalPath.TrimStart('/');
    }
}
