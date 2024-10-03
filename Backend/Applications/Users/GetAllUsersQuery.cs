using MediatR;
using UGHApi.ViewModels;

namespace UGH.Application.Users;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDTO>> { }
