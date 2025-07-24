using UGH.Domain.Interfaces;
using UGHApi.ViewModels;
using MediatR;

namespace UGHApi.Applications.Authentication;

public class UploadFilesCommandHandler : IRequestHandler<UploadFilesCommand, UploadFilesResult>
{
    private readonly S3Service _s3Uploader;
    private readonly IUserRepository _userRepository;

    public UploadFilesCommandHandler(S3Service s3Uploader, IUserRepository userRepository)
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
                linkRS = await _s3Uploader.UploadFileAsync(byteFileRS, $"{Ulid.NewUlid()}-" + request.FileRS.FileName,
                request.FileRS.ContentType);
            }
        }

        // Upload fileVS
        if (request.FileVS != null && request.FileVS.Length > 0)
        {
            using (var memoryStreamVS = new MemoryStream())
            {
                await request.FileVS.CopyToAsync(memoryStreamVS);
                var byteFileVS = memoryStreamVS.ToArray();
                linkVS = await _s3Uploader.UploadFileAsync(byteFileVS, $"{Ulid.NewUlid()}-" + request.FileVS.FileName, 
                request.FileVS.ContentType);
            }
        }

        user.Link_RS = linkRS;
        user.Link_VS = linkVS;

        await _userRepository.UpdateUserAsync(user);

        return new UploadFilesResult
        {
            LinkRS = linkRS,
            LinkVS = linkVS
        };
    }
}
