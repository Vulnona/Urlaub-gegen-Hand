using MediatR;
using Microsoft.EntityFrameworkCore;
using UGH.Contracts.Confirmation;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;

namespace UGH.Application.Admin;

public class UpdateVerifyStateCommandHandler : IRequestHandler<UpdateVerifyStateCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;
    private readonly S3Service _s3Service;
    private readonly ILogger<UpdateVerifyStateCommandHandler> _logger;
    private readonly EmailService _mailService;

    public UpdateVerifyStateCommandHandler(IUserRepository userRepository, UserService userService, S3Service s3Service,
                                           EmailService mailService, ILogger<UpdateVerifyStateCommandHandler> logger)
    {
        _userRepository = userRepository;
        _s3Service = s3Service;
        _userService = userService;
        _logger = logger;
        _mailService = mailService;
    }

    public async Task<Result> Handle(
        UpdateVerifyStateCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure(Errors.General.NotFound("UserNotFound", request.UserId));
            }

            user.VerificationState = request.VerificationState;
            await _userRepository.UpdateUserAsync(user);
            string status = request.VerificationState == UGH_Enums.VerificationState.VerificationFailed ? "Verification Failed" : "Verified";
            if (request.VerificationState != UGH_Enums.VerificationState.IsNew)
                await _mailService.SendTemplateEmailAsync(user.Email_Address, status, user.FirstName);
            //Delete from AWS after failed
            var deleteTasks = new List<Task>();

            if (!string.IsNullOrEmpty(user.Link_RS))
                deleteTasks.Add(_s3Service.DeleteFileAsync(ExtractKeyFromUrl(user.Link_RS)));

            if (!string.IsNullOrEmpty(user.Link_VS))
                deleteTasks.Add(_s3Service.DeleteFileAsync(ExtractKeyFromUrl(user.Link_VS)));

            await Task.WhenAll(deleteTasks);

            _userService.DeleteUserInfo(request.UserId);

            return Result.Success("Successfully updated verification state of user.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"Database error: {ex.Message} | {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation(
                    "Database error occurred while verifying user state"
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unhandled exception: {ex.Message} | {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation(
                    "Unexpected error occurred while verifying user state"
                )
            );
        }
    }

    private string ExtractKeyFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        return new Uri(url).LocalPath.TrimStart('/');
    }
}
