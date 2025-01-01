namespace UGHApi.Services;

public class DatabaseIntegrityChecker
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ILogger<DatabaseIntegrityChecker> _logger;

    public DatabaseIntegrityChecker(
        IServiceScopeFactory scopeFactory,
        ILogger<DatabaseIntegrityChecker> logger
    )
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    #region database-integrity
    public Task<bool> CheckIntegrityAsync()
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<Ugh_Context>();

        if (!dbContext.userroles.Any())
        {
            _logger.LogError("Critical Error: Default roles are missing in the database.");
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
    #endregion
}
