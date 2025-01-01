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

            string linkRS = user.Link_RS;
            string linkVS = user.Link_VS;

            if (!string.IsNullOrEmpty(linkRS))
            {
                var keyRS = ExtractKeyFromUrl(linkRS);
                var resultRS = await _s3Service.DeleteFileAsync(keyRS);
                _logger.LogInformation($"Deleted RS link from S3: {resultRS}");
            }

            if (!string.IsNullOrEmpty(linkVS))
            {
                var keyVS = ExtractKeyFromUrl(linkVS);
                var resultVS = await _s3Service.DeleteFileAsync(keyVS);
                _logger.LogInformation($"Deleted VS link from S3: {resultVS}");
            }

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
        return uri.AbsolutePath.TrimStart('/');
    }
}
