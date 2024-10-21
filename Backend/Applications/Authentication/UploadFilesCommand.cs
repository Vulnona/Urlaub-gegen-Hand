using MediatR;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Authentication;

public class UploadFilesCommand : IRequest<UploadFilesResult>
{
    public IFormFile FileRS { get; }
    public IFormFile FileVS { get; }
    public Guid UserId { get; }

    public UploadFilesCommand(IFormFile fileRS, IFormFile fileVS, Guid userId)
    {
        FileRS = fileRS;
        FileVS = fileVS;
        UserId = userId;
    }
}
