using MediatR;
using UGH.Domain.Interfaces;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Authentication;

public class UploadFilesCommandHandler : IRequestHandler<UploadFilesCommand, UploadFilesResult>
{
    private readonly S3Uploader _s3Uploader;
    private readonly IUserRepository _userRepository;

    public UploadFilesCommandHandler(S3Uploader s3Uploader, IUserRepository userRepository)
    {
        _s3Uploader = s3Uploader;
        _userRepository = userRepository;
    }

    public async Task<UploadFilesResult> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
    {
        string linkRS = string.Empty;
        string linkVS = string.Empty;

        var user = await _userRepository.GetUserByIdAsync(request.UserId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Upload fileRS
        if (request.FileRS != null && request.FileRS.Length > 0)
        {
            using (var memoryStreamRS = new MemoryStream())
            {
                await request.FileRS.CopyToAsync(memoryStreamRS);
                var byteFileRS = memoryStreamRS.ToArray();
                linkRS = await _s3Uploader.UploadFileAsync(byteFileRS, request.FileRS.FileName, request.FileRS.ContentType);
            }
        }

        // Upload fileVS
        if (request.FileVS != null && request.FileVS.Length > 0)
        {
            using (var memoryStreamVS = new MemoryStream())
            {
                await request.FileVS.CopyToAsync(memoryStreamVS);
                var byteFileVS = memoryStreamVS.ToArray();
                linkVS = await _s3Uploader.UploadFileAsync(byteFileVS, request.FileVS.FileName, request.FileVS.ContentType);
            }
        }

        user.SetDocumentLinks(linkRS, linkVS);
        await _userRepository.SaveChangesAsync();

        return new UploadFilesResult
        {
            LinkRS = linkRS,
            LinkVS = linkVS
        };
    }
}
