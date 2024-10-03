using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using UGH.Domain.Core;
using MediatR;
using UGH.Infrastructure.Services;

namespace UGH.Application.Offers;

public class UpdateApplicationStatusCommandHandler
    : IRequestHandler<UpdateApplicationStatusCommand, Result>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IUserRepository _userRepository;
    private readonly EmailService _emailService;
    private readonly ILogger<UpdateApplicationStatusCommandHandler> _logger;

    public UpdateApplicationStatusCommandHandler(
        IOfferRepository offerRepository,
        EmailService emailService,
        ILogger<UpdateApplicationStatusCommandHandler> logger,
        IUserRepository userRepository
    )
    {
        _offerRepository = offerRepository;
        _emailService = emailService;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(
        UpdateApplicationStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var offerApplication = await _offerRepository.GetOfferApplicationAsync(
                request.OfferId,
                request.UserId
            );

            if (offerApplication == null)
            {
                return Result.Failure(
                    Errors.General.InvalidOperation("Offer application not found.")
                );
            }

            if (offerApplication.HostId != request.HostId)
            {
                return Result.Failure(
                    Errors.General.InvalidOperation(
                        "You are not authorized to update the status of this application."
                    )
                );
            }

            offerApplication.Status = request.IsApprove
                ? OfferApplicationStatus.Approved
                : OfferApplicationStatus.Rejected;
            offerApplication.UpdatedAt = DateTime.UtcNow;

            var isUpdated = await _offerRepository.UpdateOfferApplicationAsync(offerApplication);
            if (!isUpdated)
            {
                return Result.Failure(
                    Errors.General.InvalidOperation("Failed to update offer application.")
                );
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user != null)
            {
                string userEmail = user.Email_Address;
                string status = request.IsApprove ? "approved" : "rejected";
                string subject = "Application Status Update";
                string body =
                    $"<p>Dear {user.FirstName ?? ""} {user.LastName ?? ""},</p>"
                    + $"<p>Your application for the offer has been {status} by the host.</p>"
                    + "<p>Thank you for using our service!</p>";

                Task.Run(async () =>
                    {
                        await _emailService.SendEmailAsync(userEmail, subject, body);
                        _logger.LogInformation("Notification email sent successfully to the user.");
                    })
                    .ConfigureAwait(false);
            }

            return Result.Success(
                $"Application status updated to {(request.IsApprove ? "Approved" : "Rejected")}, and notification sent to the user."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation($"Internal server error: {ex.Message}")
            );
        }
    }
}
