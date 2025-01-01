using System.Text;
using MediatR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Common;

namespace UGH.Application.Offers;

#pragma warning disable CS4014

public class AddOfferCommandHandler : IRequestHandler<AddOfferCommand, Result>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AddOfferCommandHandler> _logger;
    private readonly IConfiguration _configuration;

    public AddOfferCommandHandler(
        IOfferRepository offerRepository,
        IUserRepository userRepository,
        IConfiguration configuration,
        ILogger<AddOfferCommandHandler> logger
    )
    {
        _offerRepository = offerRepository;
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result> Handle(AddOfferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var offerViewModel = request.OfferViewModel;
            var facebookDetails = _configuration.GetSection("Facebook").Get<FacebookDetails>();

            var user = await _userRepository.GetUserWithMembershipAsync(request.UserId);

            if (user == null)
            {
                return Result.Failure(Errors.General.NotFound("UserNotFound", user));
            }

            if (user.CurrentMembership == null)
            {
                return Result.Failure(
                    Errors.General.NotFound("MembershipNotFound", user.CurrentMembership)
                );
            }

            bool isLocationProvided = !string.IsNullOrWhiteSpace(offerViewModel.Location);
            bool isCountryStateCityProvided =
                !string.IsNullOrWhiteSpace(offerViewModel.Country)
                && !string.IsNullOrWhiteSpace(offerViewModel.State)
                && !string.IsNullOrWhiteSpace(offerViewModel.City);

            if (!(isLocationProvided ^ isCountryStateCityProvided))
            {
                return Result.Failure(
                    Errors.General.InvalidOperation("Location or country state city not provided")
                );
            }

            var offer = new Offer
            {
                Title = offerViewModel.Title,
                Description = offerViewModel.Description,
                Location = offerViewModel.Location,
                CreatedAt = DateTime.UtcNow,
                Contact = offerViewModel.Contact,
                Accomodation = offerViewModel.Accommodation,
                accomodationsuitable = offerViewModel.AccommodationSuitable,
                skills = offerViewModel.Skills,
                HostId = request.UserId,
                country = offerViewModel.Country,
                state = offerViewModel.State,
                city = offerViewModel.City,
            };

            if (offerViewModel.Image.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await offerViewModel.Image.CopyToAsync(memoryStream);
                offer.ImageData = memoryStream.ToArray();
                offer.ImageMimeType = offerViewModel.Image.ContentType;
            }

            await _offerRepository.AddOfferAsync(offer);
            _logger.LogInformation("New Offer Added Successfully!");

            Task.Run(() => OpenChromeAndPerformTasks(offer, facebookDetails));

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(Errors.General.InvalidOperation("Something went wrong"));
        }
    }

    public void OpenChromeAndPerformTasks(Offer offer, FacebookDetails facebookDetails)
    {
        try
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--window-size=1920,1080");
            chromeOptions.AddArgument("--disable-software-rasterizer");
            chromeOptions.AddArgument("--remote-debugging-port=9222");
            chromeOptions.AddArgument("--disable-extensions");

            Console.WriteLine("Going to create instance");

            using (var driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl("https://mbasic.facebook.com/login.php");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                PerformLogin(wait, driver, facebookDetails);
                _logger.LogInformation("Logged in to the account successfully!");

                driver
                    .Navigate()
                    .GoToUrl($"https://mbasic.facebook.com/groups/{facebookDetails.MAIN_GROUP_ID}");
                _logger.LogInformation("Visited Group!");

                PrepareAndPostOffer(wait, driver, offer);
                _logger.LogInformation("Posted Successfully!");

                _logger.LogInformation("Logged in and posted the offer successfully!");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Exception occurred while performing the automation: {ex.Message} | StackTrace: {ex.StackTrace}"
            );
        }
    }

    private void PerformLogin(
        WebDriverWait wait,
        ChromeDriver driver,
        FacebookDetails facebookDetails
    )
    {
        var emailField = wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("email"))
        );
        emailField.SendKeys(facebookDetails.EMAIL);

        var passwordField = wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("pass"))
        );
        passwordField.SendKeys(facebookDetails.PASS);

        var loginButton = wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("login"))
        );
        loginButton.Click();

        Thread.Sleep(5000);
    }

    private void PrepareAndPostOffer(WebDriverWait wait, ChromeDriver driver, Offer offer)
    {
        var feedTextbox = wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("xc_message"))
        );

        var postMessage = CreateOfferPostMessage(offer);
        feedTextbox.SendKeys(postMessage);

        UploadPhoto(wait, driver, offer);
    }

    private void UploadPhoto(WebDriverWait wait, ChromeDriver driver, Offer offer)
    {
        wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                    By.Name("view_photo")
                )
            )
            .Click();

        var tempPath = SaveImageBytesAsFile(offer.ImageData);

        var selectPhoto = wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("//*[@id='root']/table/tbody/tr/td/form/div[1]/div/input[1]")
            )
        );
        selectPhoto.SendKeys(tempPath);

        var previewButton = wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.Name("add_photo_done")
            )
        );
        previewButton.Click();

        var postButton = wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("view_post"))
        );
        postButton.Click();
    }

    public static string SaveImageBytesAsFile(byte[] imageBytes)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), "tempImage.jpg");
        System.IO.File.WriteAllBytes(tempFilePath, imageBytes);
        return tempFilePath;
    }

    private string CreateOfferPostMessage(Offer offer)
    {
        var sb = new StringBuilder();
        sb.AppendLine("*New Offer Alert!*");
        sb.AppendLine("");
        sb.AppendLine($"Title: {offer.Title}");
        sb.AppendLine($"Description: {offer.Description}");

        if (!string.IsNullOrWhiteSpace(offer.Location))
            sb.AppendLine($"Location: {offer.Location}");

        if (!string.IsNullOrWhiteSpace(offer.Contact))
            sb.AppendLine($"Contact: {offer.Contact}");

        if (!string.IsNullOrWhiteSpace(offer.Accomodation))
            sb.AppendLine($"Accommodation Provided: {offer.Accomodation}");

        sb.AppendLine($"Skills Required: {offer.skills}");

        if (!string.IsNullOrWhiteSpace(offer.country))
            sb.AppendLine($"Country: {offer.country}");

        if (!string.IsNullOrWhiteSpace(offer.state))
            sb.AppendLine($"State: {offer.state}");

        if (!string.IsNullOrWhiteSpace(offer.city))
            sb.AppendLine($"City: {offer.city}");

        sb.AppendLine("");
        sb.AppendLine("Apply now and be a part of something amazing!");

        return sb.ToString();
    }
}
