using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;

namespace UGH.Application.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly S3Service _s3Service;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        ILogger<DeleteUserCommandHandler> logger,
        S3Service s3Service
    )
    {
        _userRepository = userRepository;
        _logger = logger;
        _s3Service = s3Service;
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

    private string ExtractKeyFromUrl(string url)
    {
        var uri = new Uri(url);
        return uri.LocalPath.TrimStart('/');
    }
}
