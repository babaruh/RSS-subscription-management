namespace TestTask.Services;

public class RefreshNewsBackgroundService : BackgroundService
{
    private readonly NewsLoaderService _newsLoaderService;
    private readonly ILogger<RefreshNewsBackgroundService> _logger;

    public RefreshNewsBackgroundService(NewsLoaderService newsLoaderService, ILogger<RefreshNewsBackgroundService> logger)
    {
        _newsLoaderService = newsLoaderService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Refreshing news...");
            await _newsLoaderService.RefreshNews();
            _logger.LogInformation("News refreshed.");

            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
        }
    }
}
