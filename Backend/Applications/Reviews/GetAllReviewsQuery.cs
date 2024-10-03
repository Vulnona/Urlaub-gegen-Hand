using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;

namespace UGH.Application.Reviews;

public class GetAllReviewsQuery : IRequest<Result<List<Review>>> { }
