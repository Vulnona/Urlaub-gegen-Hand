using UGH.Domain.Core;
using MediatR;
using UGHApi.ViewModels;

namespace UGH.Application.Admin;

public class GetAllUsersByAdminQuery : IRequest<Result<IEnumerable<UserDTO>>> { }
