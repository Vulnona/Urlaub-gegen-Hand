using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;

namespace UGHApi.Applications.Admin;

public class DeleteAdminUserCommandHandler : IRequestHandler<DeleteAdminUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteAdminUserCommandHandler> _logger;
    private readonly S3Service _s3Service;

    public DeleteAdminUserCommandHandler(IUserRepository userRepository, ILogger<DeleteAdminUserCommandHandler> logger, S3Service s3Service)
    {
        _userRepository = userRepository;
        _logger = logger;
        _s3Service = s3Service;
    }

    public async Task<Result> Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
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
                await _s3Service.DeleteFileAsync(keyRS);
            }

            if (!string.IsNullOrEmpty(linkVS))
            {
                var keyVS = ExtractKeyFromUrl(linkVS);
                await _s3Service.DeleteFileAsync(keyVS);
            }

            await _userRepository.DeleteUserAsync(user.User_Id);
            _logger.LogInformation("Admin deleted user successfully.");

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(Errors.General.InvalidOperation("An error occurred while deleting the user."));
        }
    }

    private string ExtractKeyFromUrl(string url)
    {
        var uri = new Uri(url);
        return uri.AbsolutePath.TrimStart('/');
    }
}

