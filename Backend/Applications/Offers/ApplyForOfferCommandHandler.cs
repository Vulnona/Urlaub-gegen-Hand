using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;

namespace UGH.Application.Offers;

public class ApplyForOfferCommandHandler : IRequestHandler<ApplyForOfferCommand, Result>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IUserRepository _userRepository;
    private readonly EmailService _emailService;
    private readonly ILogger<ApplyForOfferCommandHandler> _logger;

    public ApplyForOfferCommandHandler(
        IOfferRepository offerRepository,
        IUserRepository userRepository,
        EmailService emailService,
        ILogger<ApplyForOfferCommandHandler> logger
    )
    {
        _offerRepository = offerRepository;
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(
        ApplyForOfferCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure(Errors.General.NotFound("UserNotFound", user));
            }

            if (user.MembershipId == 0 || !user.IsEmailVerified)
            {
                return Result.Failure(
                    Errors.General.NotFound("NoCurrentMembership", user.CurrentMembership)
                );
            }

            var offer = await _offerRepository.GetOfferByIdAsync(request.OfferId);
            if (offer == null)
            {
                return Result.Failure(Errors.General.NotFound("OfferNotFound", offer));
            }

            if (offer.HostId == user.User_Id)
            {
                return Result.Failure(
                    Errors.General.InvalidOperation("Host Cannot apply for own offer")
                );
            }

            var existingApplication = await _offerRepository.GetOfferApplicationAsync(
                offer.Id,
                user.User_Id
            );
            if (existingApplication != null)
            {
                return Result.Failure(
                    Errors.General.InvalidOperation("Application already exists")
                );
            }

            var offerApplication = new OfferApplication
            {
                OfferId = offer.Id,
                UserId = user.User_Id,
                HostId = offer.HostId,
                Status = OfferApplicationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _offerRepository.AddOfferApplicationAsync(offerApplication);

            Task.Run(async () =>
                {
                    string hostEmail = offer.User.Email_Address;
                    string subject = "New Application for Your Offer";
                    string body =
                        $"<p>Dear {offer.User.FirstName ?? ""} {offer.User.LastName ?? ""},</p>"
                        + $"<p>Your offer has received a new application from {user.FirstName} {user.LastName}.</p>"
                        + "<p>Thank you for using our service!</p>";

                    await _emailService.SendEmailAsync(hostEmail, subject, body);
                    _logger.LogInformation("Notification email sent successfully to the host.");
                })
                .ConfigureAwait(false); // Background task ko configure karna

            _logger.LogInformation("Application submitted successfully.");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation(
                    "Something went wrong while applying for the offer."
                )
            );
        }
    }
}
