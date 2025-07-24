using MediatR;
using UGH.Domain.Core;

namespace UGH.Application.Users;

public class UploadIdCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string Link_VS { get; set; }
    public string Link_RS { get; set; }

    public UploadIdCommand(Guid userId, string linkVS, string linkRS)
    {
        UserId = userId;
        Link_VS = linkVS;
        Link_RS = linkRS;
    }
}
