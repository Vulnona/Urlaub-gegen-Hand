using Microsoft.Extensions.Logging;
using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        ILogger<DeleteUserCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
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

            await _userRepository.DeleteUserAsync(user.User_Id);

            _logger.LogInformation("User deleted successfully.");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(Errors.General.InvalidOperation("User not found."));
        }
    }
}
