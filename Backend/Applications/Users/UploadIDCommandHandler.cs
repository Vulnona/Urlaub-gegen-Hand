using Microsoft.Extensions.Logging;
using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Users;

public class UploadIdCommandHandler : IRequestHandler<UploadIdCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UploadIdCommandHandler> _logger;

    public UploadIdCommandHandler(
        IUserRepository userRepository,
        ILogger<UploadIdCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(UploadIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existUser = await _userRepository.GetUserByIdAsync(request.UserId);
            if (existUser == null)
            {
                return Result.Failure(Errors.General.InvalidOperation("User not found."));
            }

            existUser.Link_VS = request.Link_VS;
            existUser.Link_RS = request.Link_RS;
            await _userRepository.UpdateUserAsync(existUser);

            _logger.LogInformation("ID Uploaded Successfully");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(Errors.General.InvalidOperation("User not found."));
        }
    }
}
