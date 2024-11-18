namespace UGHApi.Services.AWS;

public class UrlBuilderService : IUrlBuilderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public UrlBuilderService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public string BuildAWSFileUrl(string url)
    {
        if (String.IsNullOrEmpty(url)) return null;
        var baseAwsUrl = _configuration["AwsOptions:AWS_Url"];
        var key = url.Replace(baseAwsUrl, "").Trim();

        var currentScheme = _httpContextAccessor.HttpContext?.Request?.Scheme;

        var currentHost = _httpContextAccessor.HttpContext.Request.Host;
        var fileEndpointPrefix = _configuration["FileEndpoints:FileEndpointPrefix"];
        return $"{currentScheme}://{currentHost}{fileEndpointPrefix}/{key}";
    }
}
