using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

public class ReviewUserHostedService : IHostedService, IDisposable
{
    private readonly ILogger<ReviewUserHostedService> _logger;
    private readonly IServiceProvider _services;
    private Timer _timer;

    public ReviewUserHostedService(IServiceProvider services, ILogger<ReviewUserHostedService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Review User Hosted Service running.");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        _logger.LogInformation("Review User Hosted Service is working.");

        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<UghContext>();
            try
            {
                AddPostReviewEntriesForLoginusers(context);
                AddPostReviewEntriesForOfferusers(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing reviews.");
            }
        }
    }

    private void AddPostReviewEntriesForLoginusers(UghContext context)
    {
        try
        {
            var reviewloginusers = context.reviewloginusers.ToList();

            foreach (var reviewLoginUser in reviewloginusers)
            {
                if (reviewLoginUser.CreatedAt.HasValue && reviewLoginUser.CreatedAt.Value.AddDays(14) <= DateTime.Now)
                {
                    var offerId = reviewLoginUser.OfferId;
                    var reviewOfferUser = context.reviewofferusers
                        .Where(r => r.OfferId == offerId)
                        .OrderByDescending(r => r.CreatedAt)
                        .FirstOrDefault();

                    if (reviewOfferUser == null)
                    {
                        var reviewboth = context.reviews
                            .Include(o => o.Offer)
                            .Where(o => o.OfferId == offerId && o.UserId == reviewLoginUser.UserId)
                            .FirstOrDefault();

                        if (reviewboth != null)
                        {
                            var hostId = reviewboth.Offer.User_Id;
                            var nullReviewOfOfferUser = new ReviewOfferUser
                            {
                                OfferId = offerId,
                                UserId = hostId,
                                AddReviewForOfferUser = "Host does not add Review",
                                CreatedAt = DateTime.Now
                            };

                            context.reviewofferusers.Add(nullReviewOfOfferUser);
                            context.SaveChanges();

                            reviewOfferUser = nullReviewOfOfferUser;
                        }
                    }

                    var existingPostReview = context.reviewposts
                        .FirstOrDefault(pr => pr.ReviewLoginUserId == reviewLoginUser.Id);

                    if (existingPostReview == null)
                    {
                        var postReview = new ReviewPost
                        {
                            ReviewLoginUserId = reviewLoginUser.Id,
                            LoginUserReviewPost = reviewLoginUser.AddReviewForLoginUser,
                            CreatedAt = DateTime.Now
                        };

                        if (reviewOfferUser != null)
                        {
                            postReview.ReviewOfferUserId = reviewOfferUser.Id;
                            postReview.OfferUserReviewPost = reviewOfferUser.AddReviewForOfferUser;
                        }

                        context.reviewposts.Add(postReview);
                        context.SaveChanges();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding post review entries for login users.");
        }
    }

    private void AddPostReviewEntriesForOfferusers(UghContext context)
    {
        try
        {
            var reviewofferusers = context.reviewofferusers.ToList();

            foreach (var reviewOfferUser in reviewofferusers)
            {
                if (reviewOfferUser.CreatedAt.HasValue && reviewOfferUser.CreatedAt.Value.AddMinutes(1.1) <= DateTime.Now)
                {
                    var offerId = reviewOfferUser.OfferId;
                    var reviewLoginUser = context.reviewloginusers
                        .Where(r => r.OfferId == offerId)
                        .OrderByDescending(r => r.CreatedAt)
                        .FirstOrDefault();

                    if (reviewLoginUser == null)
                    {
                        var reviewboth = context.reviews
                            .Include(o => o.Offer)
                            .Where(o => o.OfferId == offerId && o.Offer.User_Id == reviewOfferUser.UserId)
                            .FirstOrDefault();

                        if (reviewboth != null)
                        {
                            var loginUserId = reviewboth.UserId;
                            var nullReviewOfLoginUser = new ReviewLoginUser
                            {
                                OfferId = offerId,
                                UserId = loginUserId,
                                AddReviewForLoginUser = "User does not add Review",
                                CreatedAt = DateTime.Now
                            };

                            context.reviewloginusers.Add(nullReviewOfLoginUser);
                            context.SaveChanges();

                            reviewLoginUser = nullReviewOfLoginUser;
                        }
                    }

                    var existingPostReview = context.reviewposts
                        .FirstOrDefault(pr => pr.ReviewOfferUserId == reviewOfferUser.Id);

                    if (existingPostReview == null)
                    {
                        var postReview = new ReviewPost
                        {
                            ReviewOfferUserId = reviewOfferUser.Id,
                            OfferUserReviewPost = reviewOfferUser.AddReviewForOfferUser,
                            CreatedAt = DateTime.Now
                        };

                        if (reviewLoginUser != null)
                        {
                            postReview.ReviewLoginUserId = reviewLoginUser.Id;
                            postReview.LoginUserReviewPost = reviewLoginUser.AddReviewForLoginUser;
                        }

                        context.reviewposts.Add(postReview);
                        context.SaveChanges();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding post review entries for offer users.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Review User Hosted Service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
