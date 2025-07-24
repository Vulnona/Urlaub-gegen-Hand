using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;

namespace UGHApi.Applications.Memberships;

public class GetAllMembershipsQuery : IRequest<Result<List<Membership>>> { }
