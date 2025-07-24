using UGHApi.Services.HtmlTemplate;
using UGH.Infrastructure.Services;
using MediatR;

namespace UGH.Application.Authentication;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, (string Html, string MimeType)>
{
    private readonly UserService _userService;
    private readonly HtmlTemplateService _htmlTemplateService;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(
        UserService userService,
        HtmlTemplateService htmlTemplateService,
        ILogger<VerifyEmailCommandHandler> logger
    )
    {
        _userService = userService;
        _htmlTemplateService = htmlTemplateService;
        _logger = logger;
    }

    public async Task<(string Html, string MimeType)> Handle(
        VerifyEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await _userService.VerifyEmailAddressAsync(request.Token);

            if (result.IsFailure)
            {
                _logger.LogError(
                    $"Email verification failed: {result.Error.Code} - {result.Error.Message}"
                );

                if (result.Error.Code == "Error.InvalidToken")
                    return (_htmlTemplateService.GetInvalidTokenHtml(), "text/html");

                return ($"<html><body>{result.Error.Message}</body></html>", "text/html");
            }

            return (_htmlTemplateService.GetEmailVerifiedHtml(), "text/html");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception: {ex.Message} | {ex.StackTrace}");
            return (
                $"<html><body><h1>Internal Server Error</h1><p>{ex.Message}</p></body></html>",
                "text/html"
            );
        }
    }
}
