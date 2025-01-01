//using Microsoft.EntityFrameworkCore;
//using UGHApi.Models;

//public class ReviewUserHostedService : IHostedService, IDisposable
//{
//    private readonly ILogger<ReviewUserHostedService> _logger;
//    private readonly IServiceProvider _services;
//    private Timer _timer;

//    public ReviewUserHostedService(IServiceProvider services, ILogger<ReviewUserHostedService> logger)
//    {
//        _services = services;
//        _logger = logger;
//    }

//    public Task StartAsync(CancellationToken cancellationToken)
//    {
//        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(14));
//        return Task.CompletedTask;
//    }
//    #region host-review-service
//    private void DoWork(object state)
//    {
//        using (var scope = _services.CreateScope())
//        {
//            var context = scope.ServiceProvider.GetRequiredService<Ugh_Context>();
//            try
//            {
//                AddPostReviewEntriesForLoginUsers(context);
//                AddPostReviewEntriesForOfferUsers(context);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
//            }
//        }
//    }

//    private void AddPostReviewEntriesForLoginUsers(Ugh_Context context)
//    {
//        try
//        {
//            var reviewLoginUsers = context.reviewloginusers.ToList();

//            foreach (var reviewLoginUser in reviewLoginUsers)
//            {
//                if (reviewLoginUser.CreatedAt.HasValue && reviewLoginUser.CreatedAt.Value.AddDays(14) <= DateTime.UtcNow)
//                {
//                    var offerId = reviewLoginUser.OfferId;
//                    var reviewOfferUser = context.reviewofferusers
//                        .Where(r => r.OfferId == offerId)
//                        .OrderByDescending(r => r.CreatedAt)
//                        .FirstOrDefault();

//                    if (reviewOfferUser == null)
//                    {
//                        var reviewBoth = context.reviews
//                            .Include(o => o.Offer)
//                            .Where(o => o.OfferId == offerId && o.ReviewerId == reviewLoginUser.UserId)
//                            .FirstOrDefault();

//                        if (reviewBoth != null)
//                        {
//                            var hostId = reviewBoth.Offer.User_Id;
//                            var nullReviewOfOfferUser = new ReviewOfferUser
//                            {
//                                OfferId = offerId,
//                                UserId = hostId,
//                                AddReviewForOfferUser = "Host did not added the review",
//                                CreatedAt = DateTime.UtcNow
//                            };

//                            context.reviewofferusers.Add(nullReviewOfOfferUser);
//                            context.SaveChanges();

//                            reviewOfferUser = nullReviewOfOfferUser;
//                        }
//                    }

//                    var existingPostReview = context.reviewposts
//                        .FirstOrDefault(pr => pr.ReviewLoginUserId == reviewLoginUser.Id);

//                    if (existingPostReview == null)
//                    {
//                        var postReview = new ReviewPost
//                        {
//                            ReviewLoginUserId = reviewLoginUser.Id,
//                            LoginUserReviewPost = reviewLoginUser.AddReviewForLoginUser,
//                            CreatedAt = DateTime.UtcNow
//                        };

//                        if (reviewOfferUser != null)
//                        {
//                            postReview.ReviewOfferUserId = reviewOfferUser.Id;
//                            postReview.OfferUserReviewPost = reviewOfferUser.AddReviewForOfferUser;
//                        }

//                        context.reviewposts.Add(postReview);
//                        context.SaveChanges();
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
//        }
//    }

//    private void AddPostReviewEntriesForOfferUsers(Ugh_Context context)
//    {
//        try
//        {
//            var reviewOfferUsers = context.reviewofferusers.ToList();

//            foreach (var reviewOfferUser in reviewOfferUsers)
//            {
//                if (reviewOfferUser.CreatedAt.HasValue && reviewOfferUser.CreatedAt.Value.AddMinutes(1.1) <= DateTime.UtcNow)
//                {
//                    var offerId = reviewOfferUser.OfferId;
//                    var reviewLoginUser = context.reviewloginusers
//                        .Where(r => r.OfferId == offerId)
//                        .OrderByDescending(r => r.CreatedAt)
//                        .FirstOrDefault();

//                    if (reviewLoginUser == null)
//                    {
//                        var reviewBoth = context.reviews
//                            .Include(o => o.Offer)
//                            .Where(o => o.OfferId == offerId && o.Offer.User_Id == reviewOfferUser.UserId)
//                            .FirstOrDefault();

//                        if (reviewBoth != null)
//                        {
//                            var loginUserId = reviewBoth.ReviewerId;
//                            var nullReviewOfLoginUser = new ReviewLoginUser
//                            {
//                                OfferId = offerId,
//                                UserId = loginUserId,
//                                AddReviewForLoginUser = "User did not added the review",
//                                CreatedAt = DateTime.UtcNow
//                            };

//                            context.reviewloginusers.Add(nullReviewOfLoginUser);
//                            context.SaveChanges();

//                            reviewLoginUser = nullReviewOfLoginUser;
//                        }
//                    }

//                    var existingPostReview = context.reviewposts
//                        .FirstOrDefault(pr => pr.ReviewOfferUserId == reviewOfferUser.Id);

//                    if (existingPostReview == null)
//                    {
//                        var postReview = new ReviewPost
//                        {
//                            ReviewOfferUserId = reviewOfferUser.Id,
//                            OfferUserReviewPost = reviewOfferUser.AddReviewForOfferUser,
//                            CreatedAt = DateTime.UtcNow
//                        };

//                        if (reviewLoginUser != null)
//                        {
//                            postReview.ReviewLoginUserId = reviewLoginUser.Id;
//                            postReview.LoginUserReviewPost = reviewLoginUser.AddReviewForLoginUser;
//                        }

//                        context.reviewposts.Add(postReview);
//                        context.SaveChanges();
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
//        }
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        _timer?.Change(Timeout.Infinite, 0);
//        return Task.CompletedTask;
//    }

//    public void Dispose()
//    {
//        _timer?.Dispose();
//    }
//    #endregion
//}
