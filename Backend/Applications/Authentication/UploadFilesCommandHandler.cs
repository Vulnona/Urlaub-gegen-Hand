using UGH.Domain.Interfaces;
using UGHApi.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace UGHApi.Applications.Authentication;

public class UploadFilesCommandHandler : IRequestHandler<UploadFilesCommand, UploadFilesResult>
{
    private readonly S3Service _s3Uploader;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UploadFilesCommandHandler> _logger;

    public UploadFilesCommandHandler(S3Service s3Uploader, IUserRepository userRepository, ILogger<UploadFilesCommandHandler> logger)
    {
        _s3Uploader = s3Uploader;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UploadFilesResult> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: Handle aufgerufen für UserId={request.UserId}");
        
        try
        {
            // Validate user exists
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError($"[DEBUG] UploadFilesCommandHandler: User nicht gefunden für UserId={request.UserId}");
                throw new Exception($"User mit Id {request.UserId} nicht gefunden.");
            }
            _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: User geladen: Id={user.User_Id}, Email={user.Email_Address}");

            // Validate files
            if (request.FileRS == null || request.FileRS.Length == 0)
            {
                _logger.LogError("[DEBUG] UploadFilesCommandHandler: FileRS ist null oder leer");
                throw new Exception("Rückseite des Ausweises muss hochgeladen werden.");
            }

            if (request.FileVS == null || request.FileVS.Length == 0)
            {
                _logger.LogError("[DEBUG] UploadFilesCommandHandler: FileVS ist null oder leer");
                throw new Exception("Vorderseite des Ausweises muss hochgeladen werden.");
            }

            // Validate file types
            var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedContentTypes.Contains(request.FileRS.ContentType?.ToLower()))
            {
                _logger.LogError($"[DEBUG] UploadFilesCommandHandler: Ungültiger Content-Type für FileRS: {request.FileRS.ContentType}");
                throw new Exception($"Ungültiger Dateityp für Rückseite. Erlaubt sind: JPEG, PNG, GIF. Gefunden: {request.FileRS.ContentType}");
            }

            if (!allowedContentTypes.Contains(request.FileVS.ContentType?.ToLower()))
            {
                _logger.LogError($"[DEBUG] UploadFilesCommandHandler: Ungültiger Content-Type für FileVS: {request.FileVS.ContentType}");
                throw new Exception($"Ungültiger Dateityp für Vorderseite. Erlaubt sind: JPEG, PNG, GIF. Gefunden: {request.FileVS.ContentType}");
            }

            // Validate file sizes (max 10MB each)
            const int maxFileSize = 10 * 1024 * 1024; // 10MB
            if (request.FileRS.Length > maxFileSize)
            {
                _logger.LogError($"[DEBUG] UploadFilesCommandHandler: FileRS zu groß: {request.FileRS.Length} bytes");
                throw new Exception($"Rückseite ist zu groß. Maximale Größe: 10MB. Gefunden: {request.FileRS.Length / 1024 / 1024:F1}MB");
            }

            if (request.FileVS.Length > maxFileSize)
            {
                _logger.LogError($"[DEBUG] UploadFilesCommandHandler: FileVS zu groß: {request.FileVS.Length} bytes");
                throw new Exception($"Vorderseite ist zu groß. Maximale Größe: 10MB. Gefunden: {request.FileVS.Length / 1024 / 1024:F1}MB");
            }

            string linkRS = string.Empty;
            string linkVS = string.Empty;

            // Upload fileRS (Rückseite)
            _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: Starte Upload für FileRS: {request.FileRS.FileName}, Größe: {request.FileRS.Length} bytes");
            using (var memoryStreamRS = new MemoryStream())
            {
                await request.FileRS.CopyToAsync(memoryStreamRS);
                var byteFileRS = memoryStreamRS.ToArray();
                var keyNameRS = $"{Ulid.NewUlid()}-{request.FileRS.FileName}";
                
                try
                {
                    linkRS = await _s3Uploader.UploadFileAsync(byteFileRS, keyNameRS, request.FileRS.ContentType);
                    _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: FileRS erfolgreich hochgeladen: {linkRS}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[DEBUG] UploadFilesCommandHandler: Fehler beim Upload von FileRS: {ex.Message}");
                    throw new Exception($"Fehler beim Hochladen der Rückseite: {ex.Message}", ex);
                }
            }

            // Upload fileVS (Vorderseite)
            _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: Starte Upload für FileVS: {request.FileVS.FileName}, Größe: {request.FileVS.Length} bytes");
            using (var memoryStreamVS = new MemoryStream())
            {
                await request.FileVS.CopyToAsync(memoryStreamVS);
                var byteFileVS = memoryStreamVS.ToArray();
                var keyNameVS = $"{Ulid.NewUlid()}-{request.FileVS.FileName}";
                
                try
                {
                    linkVS = await _s3Uploader.UploadFileAsync(byteFileVS, keyNameVS, request.FileVS.ContentType);
                    _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: FileVS erfolgreich hochgeladen: {linkVS}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[DEBUG] UploadFilesCommandHandler: Fehler beim Upload von FileVS: {ex.Message}");
                    
                    // If VS upload fails, try to clean up the RS file
                    if (!string.IsNullOrEmpty(linkRS))
                    {
                        try
                        {
                            var keyFromUrl = linkRS.Replace(_s3Uploader.GetAwsUrl(), "");
                            await _s3Uploader.DeleteFileAsync(keyFromUrl);
                            _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: FileRS nach VS-Fehler gelöscht: {keyFromUrl}");
                        }
                        catch (Exception cleanupEx)
                        {
                            _logger.LogError($"[DEBUG] UploadFilesCommandHandler: Fehler beim Aufräumen von FileRS: {cleanupEx.Message}");
                        }
                    }
                    
                    throw new Exception($"Fehler beim Hochladen der Vorderseite: {ex.Message}", ex);
                }
            }

            // Update user with new links
            user.Link_RS = linkRS;
            user.Link_VS = linkVS;

            try
            {
                await _userRepository.UpdateUserAsync(user);
                _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: User erfolgreich aktualisiert mit neuen Links");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DEBUG] UploadFilesCommandHandler: Fehler beim Aktualisieren des Users: {ex.Message}");
                
                // If database update fails, try to clean up both files
                try
                {
                    var keyFromUrlRS = linkRS.Replace(_s3Uploader.GetAwsUrl(), "");
                    var keyFromUrlVS = linkVS.Replace(_s3Uploader.GetAwsUrl(), "");
                    await Task.WhenAll(
                        _s3Uploader.DeleteFileAsync(keyFromUrlRS),
                        _s3Uploader.DeleteFileAsync(keyFromUrlVS)
                    );
                    _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: Beide Dateien nach DB-Fehler gelöscht");
                }
                catch (Exception cleanupEx)
                {
                    _logger.LogError($"[DEBUG] UploadFilesCommandHandler: Fehler beim Aufräumen nach DB-Fehler: {cleanupEx.Message}");
                }
                
                throw new Exception($"Fehler beim Speichern der Upload-Links: {ex.Message}", ex);
            }

            var result = new UploadFilesResult
            {
                LinkRS = linkRS,
                LinkVS = linkVS
            };

            _logger.LogInformation($"[DEBUG] UploadFilesCommandHandler: Upload erfolgreich abgeschlossen für User {user.Email_Address}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[DEBUG] UploadFilesCommandHandler: Unbehandelter Fehler: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}


